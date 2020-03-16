using EditorCommon;
using UnityEditor;
using UnityEngine;

namespace ResourceFormat
{
    public class TextureViewer : BaseViewer
    {
        private TextureDataControl _control;

        public TextureViewer(EditorWindow hostWindow)
        {
            _dataTable = new TableView(hostWindow, typeof(TextureImportData));
            _dataTable.AddColumn("RootPath", "PackageRootPath", 0.26f, TextAnchor.MiddleLeft);
            _dataTable.AddColumn("FileNameMatch", "Name", 0.05f);
            _dataTable.AddColumn("Index", "Priority", 0.04f);
            _dataTable.AddColumn("TotalCount", "Count", 0.04f);
            _dataTable.AddColumn("TotalMemuse", "Memory", 0.04f, TextAnchor.MiddleCenter, "<fmt_bytes>");
            _dataTable.AddColumn("TexType", "TextureType", 0.08f);
            _dataTable.AddColumn("ShapeType", "Shape", 0.04f);
            _dataTable.AddColumn("AlphaMode", "AlphaMode", 0.05f);
            _dataTable.AddColumn("AndroidFormat", "AndroidFormat", 0.08f);
            _dataTable.AddColumn("IosFormat", "IosFormat", 0.08f);
            _dataTable.AddColumn("MaxSize", "MaxSize", 0.04f);
            _dataTable.AddColumn("ReadWriteEnable", "R/W", 0.04f);
            _dataTable.AddColumn("MipmapEnable", "Mipmap", 0.04f);
            _dataTable.AddColumn("PreBuild", "PreBuild", 0.04f);
            _dataTable.AddColumn("ForceSet", "Force", 0.04f);
            _dataTable.AddColumn("AlwaysMatch", "Always", 0.04f);
            _dataTable.SetSortParams(2, false);

            _showTable = new TableView(hostWindow, typeof(TextureInfo));
            _showTable.AddColumn("Path", "Path", 0.45f, TextAnchor.MiddleLeft);
            _showTable.AddColumn("MemSize", "Memory", 0.05f, TextAnchor.MiddleCenter, "<fmt_bytes>");
            _showTable.AddColumn("ReadWriteEnable", "R/W", 0.05f);
            _showTable.AddColumn("MipmapEnable", "Mipmap", 0.05f);
            _showTable.AddColumn("AndroidFormat", "AndroidFormat", 0.1f);
            _showTable.AddColumn("IosFormat", "IosFormat", 0.1f);
            _showTable.AddColumn("ImportType", "ImportType", 0.1f);
            _showTable.AddColumn("ImportShape", "ImportShape", 0.1f);
            _showTable.SetSortParams(1, true);

            _control = new TextureDataControl(_dataTable, _showTable);

            _dataTable.OnSelected += _control.OnDataSelected;
            _showTable.OnSelected += _control.OnInfoSelected;
        }
        public override void Draw(Rect r)
        {
            int border = TableConst.TableBorder;
            float split = TableConst.SplitterRatio;
            int toolbarHeight = 80;

            GUILayout.BeginArea(r);
            GUILayout.BeginVertical();

            _control.Draw();

            TextureImportData data = _control.Data;

            GUILayout.BeginHorizontal(TableStyles.Toolbar);
            {
                // TextureType
                GUILayout.Label("TextureType: ", GUILayout.Width(130));
                data.TextureTypeIndex = GUILayout.SelectionGrid(data.TextureTypeIndex, TableConst.TextureType, TableConst.TextureType.Length, TableStyles.ToolbarButton);

                // TextureShape
                GUILayout.Label("TextureShape: ", GUILayout.Width(130));
                data.TextureShapeIndex = GUILayout.SelectionGrid(data.TextureShapeIndex, TableConst.TextureShape, TableConst.TextureShape.Length, TableStyles.ToolbarButton);

                // MaxSize
                GUILayout.Label("MaxSize: ", GUILayout.Width(60));
                int preIndex = ArrayUtility.IndexOf(TableConst.MaxSizeInt, data.MaxSize);
                int selIndex = EditorGUILayout.Popup(preIndex, TableConst.MaxSize, TableStyles.ToolbarButton, GUILayout.MaxWidth(60));
                if (selIndex != preIndex && selIndex >= 0 && selIndex < TableConst.MaxSizeInt.Length)
                {
                    data.MaxSize = TableConst.MaxSizeInt[selIndex];
                }

                // ReadWriteEnable
                data.ReadWriteEnable = GUILayout.Toggle(data.ReadWriteEnable, " ReadWriteEnable");

                // MipMapEnable
                data.MipmapEnable = GUILayout.Toggle(data.MipmapEnable, " MipMapEnable");

                // PreBuildEnable
                data.PreBuild = GUILayout.Toggle(data.PreBuild, " PreBuildEnable");

                // ForceSet
                data.ForceSet = GUILayout.Toggle(data.ForceSet, " ForceSet");

                // AlwayMatch
                data.AlwaysMatch = GUILayout.Toggle(data.AlwaysMatch, " AlwayMatch");
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(TableConst.Space2);

            GUILayout.BeginHorizontal(TableStyles.Toolbar);
            {
                // Alphamode
                GUILayout.Label("Alphamode: ", GUILayout.Width(120));
                data.AlphaMode = (TextureAlphaMode)GUILayout.SelectionGrid((int)data.AlphaMode, TableConst.AlpaMode, TableConst.AlpaMode.Length, TableStyles.ToolbarButton);

                // AndroidFormat
                GUILayout.Label("AndroidFormat: ", GUILayout.Width(120));
                data.AndroidFormatIndex = GUILayout.SelectionGrid(data.AndroidFormatIndex, TableConst.AndoridFormat, TableConst.AndoridFormat.Length, TableStyles.ToolbarButton);

                // IosFormat
                GUILayout.Label("IosFormat: ", GUILayout.Width(120));
                data.IosFormatIndex = GUILayout.SelectionGrid(data.IosFormatIndex, TableConst.IosFormat, TableConst.IosFormat.Length, TableStyles.ToolbarButton);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(TableConst.Space2);

            GUILayout.BeginHorizontal(TableStyles.Toolbar);
            {
                // Show Unformat Data
                bool showUnFormatObject = GUILayout.Toggle(_control.Unformat, " Show Unformat Data");
                if (showUnFormatObject != _control.Unformat)
                {
                    _control.Unformat = showUnFormatObject;
                    _control.OnDataSelectedIndex();
                }

                // Move Up Priority
                if (GUILayout.Button("Move Up Priority", TableStyles.ToolbarButton, GUILayout.MaxWidth(120)))
                {
                    _control.ModifDataPriority(true);
                }

                // Move Down Priority
                if (GUILayout.Button("Move Down Priority", TableStyles.ToolbarButton, GUILayout.MaxWidth(120)))
                {
                    _control.ModifDataPriority(false);
                }

                // Apply Select Format
                if (GUILayout.Button("Apply Select Format", TableStyles.ToolbarButton, GUILayout.MaxWidth(160)))
                {
                    TextureFormater.ApplyFormatToObject(_control.SelectData);
                }

                // Refresh Select Data
                if (GUILayout.Button("Refresh Select Data", TableStyles.ToolbarButton, GUILayout.MaxWidth(140)))
                {
                    if (_control.Index != -1)
                    {
                        _control.RefreshDataByRootPath(_control.SelectData.RootPath);
                    }
                }

                // Refresh All Data
                if (GUILayout.Button("Refresh All Data", TableStyles.ToolbarButton, GUILayout.MaxWidth(140)))
                {
                    _control.RefreshBaseData();
                }

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(TableConst.Space2);

            int startY = toolbarHeight + border;
            int height = (int)(r.height - startY - border * 2);
            if (_dataTable != null)
            {
                _dataTable.Draw(new Rect(border, startY, r.width - 2 * border, (int)(height * split - border * 1.5f)));
            }

            if (_showTable != null)
            {
                _showTable.Draw(new Rect(border, (int)(height * split + border * 0.5f + startY), r.width - 2 * border, (int)(height * (1.0f - split) - border * 1.5f)));
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

    }
}
