using EditorCommon;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace ResourceFormat
{
    public class TextureOverviewViewer
    {
        // data
        protected bool[] _modeInit;
        protected List<object>[] _modeData; // object is TextureOverviewData
        protected List<TextureInfo> _texInfoList;
        protected string _rootPath = string.Empty;
        protected TextureOverviewMode _mode = TextureOverviewMode.ReadWrite;

        // view
        protected TableView _dataTable;
        protected TableView _showTable;

        public TextureOverviewViewer(EditorWindow hostWindow)
        {
            _modeInit = new bool[(int)TextureOverviewMode.Count];
            _modeData = new List<object>[(int)TextureOverviewMode.Count];
            for (int i = 0; i < (int)TextureOverviewMode.Count; ++i)
            {
                _modeInit[i] = false;
                _modeData[i] = new List<object>();
            }

            _dataTable = new TableView(hostWindow, typeof(TextureOverviewData));
            _showTable = new TableView(hostWindow, typeof(TextureInfo));

            _dataTable.OnSelected += OnDataSelected;
            _showTable.OnSelected += OnInfoSelected;

            UpdateDataTableTitle();
            UpdateShowTableTitle();
        }

        public bool SwitchMode(int mode, bool forceRefresh = false)
        {
            if (_mode == (TextureOverviewMode)mode && !forceRefresh)
            {
                return false;
            }

            _mode = (TextureOverviewMode)mode;
            if (!_modeInit[mode] && _texInfoList != null)
            {
                _modeInit[mode] = true;

                for (int i = 0; i < _texInfoList.Count; ++i)
                {
                    // 1. show progress bar
                    EditorUtility.DisplayProgressBar("刷新数据", System.IO.Path.GetFileName(_texInfoList[i].Path), (i * 1.0f) / _texInfoList.Count);

                    // 2. find data
                    bool find = false;
                    for (int j = 0; j < _modeData[mode].Count; j++)
                    {
                        TextureOverviewData overViewData = _modeData[mode][j] as TextureOverviewData;
                        if (overViewData.isMatch(_texInfoList[i]))
                        {
                            find = true;
                            overViewData.addObject(_texInfoList[i]);
                            break;
                        }
                    }

                    if (!find)
                    {
                        TextureOverviewData overViewData = TextureOverviewData.create(_mode, _texInfoList[i]);
                        overViewData.addObject(_texInfoList[i]);
                        _modeData[mode].Add(overViewData);
                    }
                }

                EditorUtility.ClearProgressBar();
            }

            //TextureOverviewData.switchDataTableMode(_mode, _dataTable);
            UpdateDataTableTitle();
            UpdateShowTableTitle();
            _dataTable.RefreshData(_modeData[mode]);
            return true;
        }

        public void RefreshData(bool forceRefresh = true)
        {
            for (int i = 0; i < _modeData.Length; ++i)
            {
                _modeData[i].Clear();
                _modeInit[i] = false;
            }

            _texInfoList = TextureInfo.GetTextureInfoByDirectory("Assets/" + _rootPath);
            SwitchMode((int)_mode, forceRefresh);
        }

        private void UpdateDataTableTitle()
        {
            float leftWide = 0.4f;
            _dataTable.ClearColumns();
            switch (_mode)
            {
                case TextureOverviewMode.ReadWrite:
                    _dataTable.AddColumn("ReadWriteEnable", "R/W Enable", leftWide);
                    break;
                case TextureOverviewMode.MipMap:
                    _dataTable.AddColumn("MipmapEnable", "MipmapEnable", leftWide);
                    break;
                case TextureOverviewMode.Type:
                    _dataTable.AddColumn("ImportType", "TextureType", leftWide);
                    break;
                case TextureOverviewMode.Resolution:
                    _dataTable.AddColumn("SizeStr", "Resolution Range", leftWide);
                    break;
                case TextureOverviewMode.WidthVSHeight:
                    _dataTable.AddColumn("WidthAndHeight", "Width VS Height", leftWide);
                    break;
                case TextureOverviewMode.StandaloneFormat:
                    _dataTable.AddColumn("StandaloneFormat", "StandaloneFormat", leftWide);
                    break;
                case TextureOverviewMode.AndroidFormat:
                    _dataTable.AddColumn("AndroidFormat", "AndroidFormat", leftWide);
                    break;
                case TextureOverviewMode.iOSFormat:
                    _dataTable.AddColumn("IosFormat", "iOSFormat", leftWide);
                    break;
            }
            _dataTable.AddColumn("Count", "Count", (1.0f - leftWide) / 2.0f);
            _dataTable.AddColumn("Memory", "Memory", (1.0f - leftWide) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>");
        }

        private void UpdateShowTableTitle()
        {
            _showTable.ClearColumns();
            switch (_mode)
            {
                case TextureOverviewMode.ReadWrite:
                case TextureOverviewMode.MipMap:
                case TextureOverviewMode.Type:
                case TextureOverviewMode.Resolution:
                case TextureOverviewMode.WidthVSHeight:
                    _showTable.AddColumn("Path", "Path", 0.8f, TextAnchor.MiddleLeft);
                    _showTable.AddColumn("MemSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>");
                    break;
                case TextureOverviewMode.StandaloneFormat:
                    _showTable.AddColumn("Path", "Path", 0.4f, TextAnchor.MiddleLeft);
                    _showTable.AddColumn("StandaloneFormat", "Format", 0.2f, TextAnchor.MiddleCenter);
                    _showTable.AddColumn("StandaloneOverriden", "Overriden", 0.2f, TextAnchor.MiddleCenter);
                    _showTable.AddColumn("StandaloneSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>");
                    break;
                case TextureOverviewMode.AndroidFormat:
                    _showTable.AddColumn("Path", "Path", 0.4f, TextAnchor.MiddleLeft);
                    _showTable.AddColumn("AndroidFormat", "Format", 0.2f, TextAnchor.MiddleCenter);
                    _showTable.AddColumn("AndroidOverriden", "Overriden", 0.2f, TextAnchor.MiddleCenter);
                    _showTable.AddColumn("AndroidSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>");
                    break;
                case TextureOverviewMode.iOSFormat:
                    _showTable.AddColumn("Path", "Path", 0.4f, TextAnchor.MiddleLeft);
                    _showTable.AddColumn("IosFormat", "Format", 0.2f, TextAnchor.MiddleCenter);
                    _showTable.AddColumn("IosOverriden", "Overriden", 0.2f, TextAnchor.MiddleCenter);
                    _showTable.AddColumn("IosSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>");
                    break;
            }
        }



        #region event
        public void OnDataSelected(object selected, int col)
        {
            TextureOverviewData overViewData = selected as TextureOverviewData;
            if (overViewData == null)
            {
                return;
            }

            if (_showTable != null)
            {
                _showTable.RefreshData(overViewData.getObject());
            }
        }

        public void OnInfoSelected(object selected, int col)
        {
            TextureInfo texInfo = selected as TextureInfo;
            if (texInfo == null)
            {
                return;
            }

            Object obj = AssetDatabase.LoadAssetAtPath(texInfo.Path, typeof(Object));
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
                    rebuild = SwitchMode(GUILayout.SelectionGrid((int)_mode, OverviewTableConst.TextureModeName, OverviewTableConst.TextureModeName.Length, TableStyles.ToolbarButton)) || rebuild;
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