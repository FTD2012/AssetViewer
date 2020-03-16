using EditorCommon;
using UnityEditor;
using UnityEngine;

namespace ResourceFormat
{
    public class AnimationViewer : BaseViewer
    {
        public AnimationViewer(EditorWindow hostWindow)
        {
            // m_hostWindow = hostWindow;

            // create the table with a specified object type
            _dataTable = new TableView(hostWindow, typeof(AnimationImportData));

            // setup the description for content
            _dataTable.AddColumn("RootPath", "PackageRootPath", 0.35f, TextAnchor.MiddleLeft);
            _dataTable.AddColumn("FileNameMatch", "Name", 0.05f);
            _dataTable.AddColumn("Index", "Priority", 0.05f);
            _dataTable.AddColumn("TotalCount", "Count", 0.05f);
            _dataTable.AddColumn("TotalMemuse", "Memory", 0.1f, TextAnchor.MiddleCenter, "<fmt_bytes>");
            _dataTable.AddColumn("AnimationType", "AnimationType", 0.2f);
            _dataTable.AddColumn("AnimationCompression", "AnimationCompression", 0.2f);


            // sorting
            _dataTable.SetSortParams(2, false);

            _showTable = new TableView(hostWindow, typeof(AnimationInfo));

            _showTable.AddColumn("Path", "Path", 0.5f, TextAnchor.MiddleLeft);
            _showTable.AddColumn("MemSize", "Memory", 0.1f, TextAnchor.MiddleCenter, "<fmt_bytes>");
            _showTable.AddColumn("AnimationType", "AnimationType", 0.2f);
            _showTable.AddColumn("AnimationCompression", "AnimationCompression", 0.2f);

            _showTable.SetSortParams(1, true);

            m_control = new AnimationDataControl(_dataTable, _showTable);

            // register the event-handling function
            _dataTable.OnSelected += m_control.OnDataSelected;
            _showTable.OnSelected += m_control.OnInfoSelected;
        }
        public override void Draw(Rect r)
        {
            int border = TableConst.TableBorder;
            float split = TableConst.SplitterRatio;
            int toolbarHeight = 80;

            GUILayout.BeginArea(r);
            GUILayout.BeginVertical();

            m_control.Draw();

            AnimationImportData data = m_control.Data;

            GUILayout.BeginHorizontal(TableStyles.Toolbar);
            {
                GUILayout.Label("AnimationType: ", GUILayout.Width(120));
                int aniType = GUILayout.SelectionGrid((int)data.AnimationType,
                    TableConst.AnimationType, TableConst.AnimationType.Length, TableStyles.ToolbarButton);
                if (aniType != (int)data.AnimationType)
                {
                    data.AnimationType = (ModelImporterAnimationType)aniType;
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(TableStyles.Toolbar);
            {
                GUILayout.Label("AnimationCompression: ", GUILayout.Width(120));
                int aniCompression = GUILayout.SelectionGrid((int)data.AnimationCompression,
                    TableConst.AnimationCompression, TableConst.AnimationCompression.Length, TableStyles.ToolbarButton);
                if (aniCompression != (int)data.AnimationCompression)
                {
                    data.AnimationCompression = (ModelImporterAnimationCompression)aniCompression;
                }

            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(TableStyles.Toolbar);
            {
                bool showUnFormatObject = GUILayout.Toggle(m_control.UnFormat, " Show Unformat Data");
                if (showUnFormatObject != m_control.UnFormat)
                {
                    m_control.UnFormat = showUnFormatObject;
                    m_control.OnDataSelectedIndex();
                }

                if (GUILayout.Button("Move Up Priority", TableStyles.ToolbarButton, GUILayout.MaxWidth(120)))
                {
                    m_control.ModifDataPriority(true);
                }

                if (GUILayout.Button("Move Down Priority", TableStyles.ToolbarButton, GUILayout.MaxWidth(120)))
                {
                    m_control.ModifDataPriority(false);
                }

                if (GUILayout.Button("Apply Select Format", TableStyles.ToolbarButton, GUILayout.MaxWidth(160)))
                {
                    AnimationFormater.ApplyFormatToObject(m_control.SelectData);
                }

                if (GUILayout.Button("Refresh Select Data", TableStyles.ToolbarButton, GUILayout.MaxWidth(140)))
                {
                    if (m_control.Index != -1)
                    {
                        m_control.RefreshDataByRootPath(m_control.SelectData.RootPath);
                    }
                }

                if (GUILayout.Button("Refresh All Data", TableStyles.ToolbarButton, GUILayout.MaxWidth(140)))
                {
                    m_control.RefreshBaseData();
                }

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

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

        private AnimationDataControl m_control;
    }
}
