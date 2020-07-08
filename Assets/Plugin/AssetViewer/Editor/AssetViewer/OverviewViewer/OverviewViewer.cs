using EditorCommon;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using UnityEngine.Assertions;
using CommonComponent;
using System.Reflection;
using System.Text;

namespace AssetViewer
{
    public class OverviewViewer<T, U, V, H> where T : OverviewData where U : BaseInfo where V : OverviewModeManager where H : HealthInfoManager
    {
        // data
        protected Dictionary<string, bool> _modeInit;
        protected Dictionary<string, List<object>> _modeData; // object is XXXOverviewData
        protected Dictionary<string, Health> _modeHealth;
        protected List<U> _infoList;
        protected string _rootPath = string.Empty;
        protected string _mode;

        // view
        protected TableView _dataTable;
        protected TableView _showTable;

        // meta-data
        protected V _overviewModeManager;
        protected H _healthInfoManager;

        public OverviewViewer(EditorWindow hostWindow)
        {
            Assert.IsNotNull(hostWindow);

            _overviewModeManager = OverviewTableConst.GetSingletonInstance<V>();
            _healthInfoManager = OverviewTableConst.GetSingletonInstance<H>();

            _modeInit = new Dictionary<string, bool>();
            _modeData = new Dictionary<string, List<object>>();
            _modeHealth = new Dictionary<string, Health>();
            _mode = _overviewModeManager.GetMode()[0];
            foreach (var key in _overviewModeManager.GetMode())
            {
                _modeInit[key] = false;
                _modeData[key] = new List<object>();
            }

            _dataTable = new TableView(hostWindow, typeof(T));
            _showTable = new TableView(hostWindow, typeof(U));

            _dataTable.OnSelected += OnDataSelected;
            _showTable.OnSelected += OnInfoSelected;

            UpdateDataTableTitle();
            UpdateShowTableTitle();
        }

        public bool SwitchMode(string mode, bool forceRefresh = false)
        {
            if (_mode == mode && !forceRefresh)
            {
                return false;
            }

            _mode = mode;
            if (!_modeInit[mode] && _infoList != null)
            {
                _modeInit[mode] = true;

                for (int i = 0; i < _infoList.Count; ++i)
                {
                    // 1. show progress bar
                    EditorUtility.DisplayProgressBar("刷新数据", System.IO.Path.GetFileName(_infoList[i].Path), (i * 1.0f) / _infoList.Count);

                    // 2. find data
                    bool find = false;
                    for (int j = 0; j < _modeData[mode].Count; j++)
                    {
                        T overViewData = _modeData[mode][j] as T;
                        if (overViewData.IsMatch(_infoList[i]))
                        {
                            find = true;
                            overViewData.AddObject(_infoList[i]);
                            break;
                        }
                    }

                    if (!find)
                    {
                        T overViewData = (T)Activator.CreateInstance(typeof(T), _mode, _infoList[i]);
                        overViewData.AddObject(_infoList[i]);
                        _modeData[mode].Add(overViewData);
                    }
                }

                if (_healthInfoManager.GetEnableCondition(mode))
                {
                    int validCount = 0; // config
                    foreach (T overViewData in _modeData[mode])
                    {
                        foreach (object condition in _healthInfoManager.GetConditionList(mode))
                        {
                            validCount += overViewData.GetMatchHealthCount(condition);
                        }
                    }
                    _modeHealth[mode] = new Health(OverviewTableConst.GetHealthState(_healthInfoManager.GetThreshold(mode), validCount), _healthInfoManager.GetTip(mode), _healthInfoManager.GetThreshold(mode), validCount);
                }
                else
                {
                    _modeHealth[mode] = new Health(Health.HealthEnum.NONE, _healthInfoManager.GetTip(mode), 0, 0);
                }


                EditorUtility.ClearProgressBar();
            }

            UpdateDataTableTitle();
            UpdateShowTableTitle();
            _dataTable.RefreshData(_modeData[mode]);
            return true;
        }

        public void RefreshData(bool forceRefresh = true)
        {
            foreach(var key in _modeData.Keys)
            {
                _modeData[key].Clear();
                _modeInit[key] = false;
            }

            _infoList = (List<U>)typeof(U).GetMethod("GetInfoByDirectory", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { "Assets/" + _rootPath });

            SwitchMode(_mode, forceRefresh);
        }

        private void UpdateDataTableTitle()
        {
            _dataTable.ClearColumns();

            foreach (var columnType in _overviewModeManager.GetDataTable(_mode))
            {
                _dataTable.AddColumn(columnType.colDataPropertyName, columnType.colTitleText, columnType.widthByPercent, columnType.alignment, columnType.fmt);
            }
        }

        private void UpdateShowTableTitle()
        {
            _showTable.ClearColumns();

            foreach (var columnType in _overviewModeManager.GetShowTable(_mode))
            {
                _showTable.AddColumn(columnType.colDataPropertyName, columnType.colTitleText, columnType.widthByPercent, columnType.alignment, columnType.fmt);
            }
        }

        #region event
        public void OnDataSelected(object selected, int col)
        {
            T overViewData = selected as T;
            if (overViewData == null)
            {
                return;
            }

            if (_showTable != null)
            {
                _showTable.RefreshData(overViewData.GetObject());
            }
        }

        public void OnInfoSelected(object selected, int col)
        {
            U detailInfo = selected as U;
            if (detailInfo == null)
            {
                return;
            }

            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(detailInfo.Path, typeof(UnityEngine.Object));
            EditorGUIUtility.PingObject(obj);
            Selection.activeObject = obj;
        }
        #endregion event

        public void Draw(Rect rect, KeyCode keyCode)
        {
            bool rebuild = false;

            GUILayout.BeginVertical();
            {
                // 1. root path
                GUILayout.BeginHorizontal(TableStyles.Toolbar);
                {
                    GUILayout.Label("RootPath: ", GUILayout.Width(100));
                    _rootPath = EditorGUILayout.TextField(_rootPath, TableStyles.TextField, GUILayout.Width(360));
                    Color origColor = GUI.backgroundColor;
                    GUI.backgroundColor = Color.green;
                    if (GUILayout.Button("Refresh Data", TableStyles.ToolbarButton, GUILayout.MaxWidth(120)) || keyCode == KeyCode.F5)
                    {
                        rebuild = true;
                        RefreshData(forceRefresh: true);
                    }
                    GUI.backgroundColor = origColor;

                    // drop down
                    //GUILayout.FlexibleSpace();
                    EditorGUILayout.PrefixLabel("Threshod Selector", EditorStyles.miniButton);
                    var modeContent = new GUIContent("Selector \u007C \u25BE");
                    var modeRect = GUILayoutUtility.GetRect(modeContent, EditorStyles.miniButton, GUILayout.ExpandWidth(false));
                    var modeDropRect = new Rect(modeRect.xMax - 16, modeRect.y, 16, modeRect.height);

                    if (EditorGUI.DropdownButton(modeDropRect, GUIContent.none, FocusType.Passive, GUIStyle.none) || GUI.Button(modeRect, modeContent, EditorStyles.miniButton))
                    {
                        PopupWindow.Show(modeRect, new HealthConfigPopup());
                    }

                }
                GUILayout.EndHorizontal();

                // 2. mode
                GUILayout.BeginHorizontal(TableStyles.Toolbar);
                {
                    GUILayout.Label("Mode: ", GUILayout.Width(100));
                    string[] mode = _overviewModeManager.GetMode();
                    rebuild = SwitchMode(mode[GUILayout.SelectionGrid(Array.IndexOf(mode, _mode), mode, mode.Length, TableStyles.ToolbarButton)]) || rebuild;
                }
                GUILayout.EndHorizontal();

                // 3. health
                int healthHeight = 38;
                {
                    if (_modeHealth.ContainsKey(_mode))
                    {
                        StringBuilder sb = new StringBuilder(100);
                        sb.Append(OverviewTableConst.GetHealthStateDesc(_modeHealth[_mode].State));
                        sb.Append(_modeHealth[_mode].Desc);
                        if (_modeHealth[_mode].State != Health.HealthEnum.NONE)
                        {
                            if (_modeHealth[_mode].Threshold > 0)
                            {
                                sb.Append(string.Format(OverviewTableString.Recommand, _modeHealth[_mode].Threshold, _modeHealth[_mode].Value));
                            }
                            else
                            {
                                sb.Append(string.Format(OverviewTableString.RecommandCurrent, _modeHealth[_mode].Value));
                            }
                        }
                        else
                        {
                            healthHeight = 17;
                        }

                        if (sb.Length != 0)
                        {
                            EditorGUILayout.HelpBox(sb.ToString(), (MessageType)Enum.ToObject(typeof(MessageType), (int)_modeHealth[_mode].State));
                        }
                        else
                        {
                            healthHeight = 0;
                        }
                    }
                    else
                    {
                        EditorGUILayout.HelpBox(OverviewTableString.NotInitTip, MessageType.Warning);
                    }
                }

                // 4. select area
                {
                    if (_modeInit[_mode])
                    {
                        float startY = TableConst.RootPathHeight + TableConst.ModeHeight + TableConst.TableBorder + healthHeight;
                        float height = rect.height - startY - 5;
                        _dataTable.Draw(new Rect(TableConst.TableBorder, startY, rect.width * TableConst.SplitterRatio - 1.5f * TableConst.TableBorder, height), rebuild);
                        _showTable.Draw(new Rect(rect.width * TableConst.SplitterRatio + 0.5f * TableConst.TableBorder, startY, rect.width * (1.0f - TableConst.SplitterRatio) - 1.5f * TableConst.TableBorder, height));
                    }
                }
            }
            GUILayout.EndVertical();
        }

    }

}