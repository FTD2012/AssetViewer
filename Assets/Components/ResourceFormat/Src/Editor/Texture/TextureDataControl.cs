using EditorCommon;
using System.Collections.Generic;
using UnityEditor;

namespace ResourceFormat
{
    public class TextureDataControl : FormatDataControl<TextureImportData>
    {
        private bool _showUnformatObject = false;
        private List<TextureInfo> _texInfoList = new List<TextureInfo>();

        public TextureDataControl(TableView dataTable, TableView showTable)
        {
            _dataTable = dataTable;
            _showTable = showTable;

            _dataList = ConfigData.TextureImportData;
            base.RefreshDataWithSelect();
        }

        public bool Unformat
        {
            get { return _showUnformatObject; }
            set { _showUnformatObject = value; }
        }

        protected override void RefreshList(List<string> list)
        {
            _texInfoList = new List<TextureInfo>();
            for (int i = 0; i < list.Count; ++i)
            {
                string path = EditorPath.FormatAssetPath(list[i]);
                string name = System.IO.Path.GetFileName(path);
                EditorUtility.DisplayProgressBar("获取贴图数据", name, (i * 1.0f) / list.Count);
                if (!EditorPath.IsTexture(path))
                {
                    continue;
                }

                TextureInfo texInfo = TextureInfo.CreateTextureInfo(path);
                if (texInfo != null)
                {
                    _texInfoList.Add(texInfo);
                }
            }
            EditorUtility.ClearProgressBar();
            RefreshDataWithSelect();
        }

        public override void RefreshDataWithSelect()
        {
            base.RefreshDataWithSelect();
            if (_texInfoList != null)
            {
                for (int i = 0; i < _texInfoList.Count; i++)
                {
                    string name = System.IO.Path.GetFileName(_texInfoList[i].Path);
                    EditorUtility.DisplayProgressBar("更新贴图表数据", name, (i * 1.0f) / _texInfoList.Count);
                    for (int j = _dataList.Count - 1; j >= 0; --j)
                    {
                        if (_dataList[j].IsMatch(_texInfoList[i].Path))
                        {
                            _dataList[j].AddObject(_texInfoList[i]);
                        }
                    }
                }
                EditorUtility.ClearProgressBar();
            }

        }
        public override void OnDataSelected(object selected, int col)
        {
            TextureImportData texSelectData = selected as TextureImportData;
            if (texSelectData == null)
            {
                return;
            }

            _editorData.CopyData(texSelectData);
            _index = texSelectData.Index;
            if (texSelectData != null)
            {
                _showTable.RefreshData(texSelectData.GetObject(_showUnformatObject));
            }
        }

    }
}
