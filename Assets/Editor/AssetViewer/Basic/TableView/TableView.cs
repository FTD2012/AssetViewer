using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace EditorCommon
{
    public delegate void SelectionHandler(object selected, int col);

    public struct ColumnType 
    {
        public string colDataPropertyName;
        public string colTitleText;
        public float widthByPercent;
        public TextAnchor alignment;
        public string fmt;

        public ColumnType(string _colDataPropertyName, string _colTitleText, float _widthByPercent, TextAnchor _alignment, string _fmt)
        {
            colDataPropertyName = _colDataPropertyName;
            colTitleText = _colTitleText;
            widthByPercent = _widthByPercent;
            alignment = _alignment;
            fmt = _fmt;
        }
    }


    public partial class TableView : IDisposable
    {
        // show internal sequential ID in the first column
        public bool ShowInternalSeqID = true;

        public event SelectionHandler OnSelected;

        public TableViewAppr Appearance { get { return _appearance; } }

        public TableView(EditorWindow hostWindow, Type itemType)
        {
            _hostWindow = hostWindow;
            _itemType = itemType;
        }

        public void Dispose()
        {

        }

        public void ClearColumns()
        {
            _descArray.Clear();
        }

        public bool AddColumn(string colDataPropertyName, string colTitleText, float widthByPercent, TextAnchor alignment = TextAnchor.MiddleCenter, string fmt = "")
        {
            TableViewColDesc desc = new TableViewColDesc();
            desc.PropertyName = colDataPropertyName;
            desc.TitleText = colTitleText;
            desc.Alignment = alignment;
            desc.WidthInPercent = widthByPercent;
            desc.Format = string.IsNullOrEmpty(fmt) ? null : fmt;
            desc.MemInfo = _itemType.GetField(desc.PropertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
            if (desc.MemInfo == null)
            {
                desc.MemInfo = _itemType.GetProperty(desc.PropertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);
                if (desc.MemInfo == null)
                {
                    Debug.LogWarningFormat("Field '{0}' accessing failed.", desc.PropertyName);
                    return false;
                }
            }

            _descArray.Add(desc);
            return true;
        }

        public void RefreshData(List<object> entries, Dictionary<object, Color> specialTextColors = null)
        {
            _lines.Clear();
            if (entries != null && entries.Count > 0)
            {
                _lines.AddRange(entries);
                SortData();
            }
            _specialTextColors = specialTextColors;
        }

        public void Draw(Rect area, bool rebuild = false)
        {
            GUILayout.BeginArea(area);
            DrawTitle(area.width - 15);

            GUIStyle guiStyle = new GUIStyle();
            guiStyle.fixedHeight = _appearance.LineHeight * (_lines.Count + 1);
            guiStyle.stretchWidth = true;

            GUILayout.BeginArea(new Rect(0, _appearance.LineHeight, area.width, area.height));
            _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUIStyle.none, GUI.skin.verticalScrollbar);
            //Debug.LogFormat("scroll pos: {0:0.00}, {1:0.00}", _scrollPos.x, _scrollPos.y);
            {
                Rect r = EditorGUILayout.BeginVertical(guiStyle);
                {
                    // this silly line (empty label) is required by Unity to ensure the scroll bar appear as expected.
                    PAEditorUtil.DrawLabel("", _appearance.StyleLine);

                    // these first/last calculatings are for smart clipping 
                    int firstLine = Mathf.Max((int)(_scrollPos.y / _appearance.LineHeight) - 1, 0);
                    int shownLineCount = (int)(area.height / _appearance.LineHeight) + 2;
                    int lastLine = Mathf.Min(firstLine + shownLineCount, _lines.Count);

                    for (int i = firstLine; i < lastLine; i++)
                    {
                        DrawLine(i, _lines[i], r.width, rebuild);
                    }
                }
                EditorGUILayout.EndVertical();
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();
            GUILayout.EndArea();
        }

        public void SetSortParams(int sortSlot, bool descending)
        {
            _sortSlot = sortSlot;
            _descending = descending;
        }

        public void SetSelected(object obj)
        {
            _selected = obj;
            if (OnSelected != null)
            {
                OnSelected(obj, 0);
            }
        }

        public object GetSelected()
        {
            return _selected;
        }
    }
}