using EditorCommon;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ResourceFormat
{
    public class FormatDataControl<T> where T : ImportData, new()
    {
        protected TableView _dataTable;
        protected TableView _showTable;

        protected int _index = -1;
        protected T _editorData = new T();
        protected List<T> _dataList = null;

        public int Index
        {
            get { return _index; }
            //set { _index = value; }
        }

        public T Data
        {
            get { return _editorData; }
            //set { _editorData = value; }
        }

        public T SelectData
        {
            get { return _index == -1 ? null : _dataList[_index]; }
        }

        public List<T> DataList
        {
            get { return _dataList; }
        }

        public virtual void OnDataSelected(object selected, int col)
        {
            ImportData importData = selected as ImportData;
            if (importData == null)
            {
                return;
            }

            _editorData.CopyData(importData);
            _index = importData.Index;
            if (importData != null && _showTable != null)
            {
                _showTable.RefreshData(importData.GetObject());
            }
        }
        public virtual void OnInfoSelected(object selected, int col)
        {
            BaseInfo texInfo = selected as BaseInfo;
            if (texInfo == null)
            {
                return;
            }
            Object obj = AssetDatabase.LoadAssetAtPath(texInfo.Path, typeof(Object));
            EditorGUIUtility.PingObject(obj);
            Selection.activeObject = obj;
        }

        public virtual void AddData()
        {
            _editorData.Index = _dataList.Count;
            _dataList.Add(_editorData);
            _editorData = new T();
            _index = -1;
            ConfigData.SaveData();
            RefreshDataWithSelect();
        }

        public virtual void SaveData()
        {
            if (_index == -1)
            {
                return;
            }

            T data = _dataList[_index];
            data.ClearObject();
            data.CopyData(_editorData);
            OnDataSelected(data, _index);
            ConfigData.SaveData();
            RefreshDataWithSelect();
        }

        public virtual void DeleteCurrentData()
        {
            if (_index == -1)
            {
                return;
            }

            _dataList.RemoveAt(_index);
            _index = -1;
            _editorData = new T();
            ConfigData.SaveData();
            RefreshDataWithSelect();
        }

        public virtual void ModifDataPriority(bool up)
        {
            if (_index == -1)
            {
                return;
            }

            var temp = _dataList[_index];
            if (up)
            {
                if (_index == 0)
                {
                    return;
                }

                _dataList[_index] = _dataList[_index - 1];
                _dataList[_index - 1] = temp;
            }
            else
            {
                if (_index + 1 == _dataList.Count)
                {
                    return;
                }
                _dataList[_index] = _dataList[_index + 1];
                _dataList[_index + 1] = temp;
            }
            ConfigData.SaveData();

            RefreshDataWithSelect();
            if (_dataTable != null)
            {
                _dataTable.SetSelected(temp);
            }
        }

        public virtual void NewData()
        {
            _index = -1;
            _editorData = new T();
        }

        public virtual void RefreshBaseData()
        {
            List<string> list = EditorPath.GetAssetPathList(FormatConfig.ResourceRootPath);
            RefreshList(list);
        }

        public virtual void RefreshDataByRootPath(string path)
        {
            List<string> list = EditorPath.GetAssetPathList(FormatConfig.ResourceRootPath + "/" + path);
            RefreshList(list);
        }

        protected virtual void RefreshList(List<string> list)
        {

        }

        public virtual void OnDataSelectedIndex()
        {
            if (_index == -1)
            {
                return;
            }

            OnDataSelected(_dataList[_index], _index);
        }

        public virtual void RefreshDataWithSelect()
        {
            for (int i = 0; i < _dataList.Count; ++i)
            {
                _dataList[i].ClearObject();
                _dataList[i].Index = i;
            }

            if (_dataTable != null)
            {
                _dataTable.RefreshData(EditorTool.ToObjectList<T>(_dataList));
            }
        }

        public virtual void Draw()
        {
            T data = _editorData;

            GUILayout.BeginHorizontal(TableStyles.Toolbar);
            {
                GUILayout.FlexibleSpace();

                GUILayout.Label("RootPath: ", GUILayout.Width(80));
                data.RootPath = EditorGUILayout.TextField(data.RootPath, TableStyles.TextField, GUILayout.Width(360));

                GUILayout.FlexibleSpace();

                GUILayout.Label("FileName: ", GUILayout.Width(60));
                data.FileNameMatch = EditorGUILayout.TextField(data.FileNameMatch, TableStyles.TextField, GUILayout.Width(150));
                GUILayout.FlexibleSpace();

                if (Index == -1)
                {
                    GUI.backgroundColor = Color.green;
                    if (GUILayout.Button("Add Data", TableStyles.ToolbarButton, GUILayout.MaxWidth(120)))
                    {
                        AddData();
                    }
                    GUI.backgroundColor = Color.white;
                }
                else
                {
                    GUI.backgroundColor = Color.green;
                    if (GUILayout.Button("Save Data", TableStyles.ToolbarButton, GUILayout.MaxWidth(140)))
                    {
                        SaveData();
                    }

                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Delete Current Data", TableStyles.ToolbarButton, GUILayout.MaxWidth(140)))
                    {
                        DeleteCurrentData();
                    }

                    GUI.backgroundColor = Color.white;
                    if (GUILayout.Button("New Data", TableStyles.ToolbarButton, GUILayout.MaxWidth(120)))
                    {
                        NewData();
                    }
                }
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(TableConst.Space6);
        }

    }
}
