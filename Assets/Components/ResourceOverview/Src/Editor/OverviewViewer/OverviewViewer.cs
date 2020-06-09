using EditorCommon;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using UnityEngine.Assertions;
using CommonComponent;
using System.Reflection;

namespace ResourceFormat
{
    public class OverviewViewer<T, U, V> where T : OverviewData where U : BaseInfo where V : OverviewModeManager
    {
        // data
        protected Dictionary<string, bool> _modeInit;
        protected Dictionary<string, List<object>> _modeData; // object is XXXOverviewData
        protected List<U> _infoList;
        protected string _rootPath = string.Empty;
        protected string _mode;

        // view
        protected TableView _dataTable;
        protected TableView _showTable;

        // meta-data
        protected V _overviewModeManager;

        public OverviewViewer(EditorWindow hostWindow)
        {
            Assert.IsNotNull(hostWindow);

            _overviewModeManager = (V)typeof(Singleton<>).MakeGenericType(typeof(V)).GetMethod("Instance", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);

            _modeInit = new Dictionary<string, bool>();
            _modeData = new Dictionary<string, List<object>>();
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
            GUILayout.BeginVertical();
            {
                bool rebuild = false;

                // 1. root path
                GUILayout.BeginHorizontal(TableStyles.Toolbar);
                {
                    GUILayout.Label("RootPath: ", GUILayout.Width(100));
                    _rootPath = EditorGUILayout.TextField(_rootPath, TableStyles.TextField, GUILayout.Width(360)); /// TODO: ljm >>> change to select path
                    if (GUILayout.Button("Refresh Data", TableStyles.ToolbarButton, GUILayout.MaxWidth(120)) || keyCode == KeyCode.F5)
                    {
                        rebuild = true;
                        RefreshData(forceRefresh: true);
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

                // 3. select area
                {
                    float startY = TableConst.RootPathHeight + TableConst.ModeHeight + TableConst.TableBorder;
                    float height = rect.height - startY - 5;
                    _dataTable.Draw(new Rect(TableConst.TableBorder, startY, rect.width * TableConst.SplitterRatio - 1.5f * TableConst.TableBorder, height), rebuild);
                    _showTable.Draw(new Rect(rect.width * TableConst.SplitterRatio + 0.5f * TableConst.TableBorder, startY, rect.width * (1.0f - TableConst.SplitterRatio) - 1.5f * TableConst.TableBorder, height));
                }
            }
            GUILayout.EndVertical();
        }

    }

}