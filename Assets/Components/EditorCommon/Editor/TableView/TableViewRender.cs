using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
namespace EditorCommon
{
    public partial class TableView
    {
        private int _sortSlot = 0;
        private int _selectedCol = -1;
        private object _selected = null;
        private bool _descending = true;

        private Type _itemType = null;
        private EditorWindow _hostWindow = null;
        private Vector2 _scrollPos = Vector2.zero;
        private List<object> _lines = new List<object>();
        private TableViewAppr _appearance = new TableViewAppr();
        private Dictionary<object, Color> _specialTextColors = null;
        private List<TableViewColDesc> _descArray = new List<TableViewColDesc>();

        public bool Descending
        {
            get { return _descending; }
            set { _descending = value; }
        }

        private Rect LabelRect(float width, int slot, int pos)
        {
            float accumPercent = 0.0f;
            int count = Mathf.Min(slot, _descArray.Count);
            for (int i = 0; i < count; i++)
            {
                accumPercent += _descArray[i].WidthInPercent;
            }
            return new Rect(width * accumPercent, pos * _appearance.LineHeight, width * _descArray[slot].WidthInPercent, _appearance.LineHeight);
        }

        private void SortData()
        {
            _lines.Sort((s1, s2) => { return (_sortSlot >= _descArray.Count) ?  0 : _descArray[_sortSlot].Compare(s1, s2) * (_descending ? -1 : 1); });
        }

        private void DrawTitle(float width)
        {
            for (int i = 0; i < _descArray.Count; i++)
            {
                // title label
                GUI.Label(LabelRect(width, i, 0), _descArray[i].TitleText + (_sortSlot == i ? _appearance.GetSortMark(_descending) : ""), _appearance.GetTitleStyle(_sortSlot == i));

                // click event
                if (EditorTool.MouseClickInRect(LabelRect(width, i, 0)))
                {
                    if (_sortSlot == i)
                    {
                        _descending = !_descending;
                    }
                    else
                    {
                        _sortSlot = i;
                    }
                    SortData();
                    _hostWindow.Repaint();
                }
            }
        }

        private void DrawLine(int pos, object obj, float width)
        {
            bool selectionHappens = EditorTool.MouseClickInRect(new Rect(0, pos * _appearance.LineHeight, width, _appearance.LineHeight));
            GUIStyle style = new GUIStyle((pos % 2 != 0) ? _appearance.StyleLine : _appearance.StyleLineAlt);

            // click event
            if (selectionHappens)
            {
                _selected = obj;
            }

            // note that the 'selected-style' assignment below should be isolated from the if-conditional statement above
            // since the above if is a one-time event, on the contrary, the 'selected-style' assignment below should be done every time in the drawing process
            if (_selected == obj)
            {
                style = _appearance.StyleSelected;
            }
            else
            {
                Color specialColor;
                if (_specialTextColors != null && _specialTextColors.TryGetValue(obj, out specialColor))
                {
                    style.normal.textColor = specialColor;
                }
            }

            // draw line column
            for (int i = 0; i < _descArray.Count; i++)
            {
                DrawLineCol(pos, i, width, obj, style, selectionHappens);
            }
        }

        private void DrawLineCol(int pos, int col, float width, object obj, GUIStyle style, bool selectionHappens = false)
        {
            string text = _descArray[col].FormatObject(obj);

            // set selection area
            if (selectionHappens)
            {
                _selectedCol = col;
                if (OnSelected != null)
                {
                    OnSelected(obj, col);
                }

                EditorGUIUtility.systemCopyBuffer = text;
                _hostWindow.Repaint();
            }

            // internal sequential id
            if (ShowInternalSeqID && col == 0)
            {
                text = pos.ToString() + ". " + text;
            }

            // note that the 'selected-style' assignment below should be isolated from the if-conditional statement above
            // since the above if is a one-time event, on the contrary, the 'selected-style' assignment below should be done every time in the drawing process
            if (_selectedCol == col && _selected == obj)
            {
                style = _appearance.StyleSelectedCell;
            }

            style.alignment = _descArray[col].Alignment;
            GUI.Label(LabelRect(width, col, pos), new GUIContent(text, text), style);
        }
    }
}