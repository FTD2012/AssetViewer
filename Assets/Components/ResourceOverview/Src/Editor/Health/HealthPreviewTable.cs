using EditorCommon;
using ResourceFormat;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HealthPreviewTable
{

    protected List<object> _healthPreivewData; // object is XXXOverviewData
    protected TableView _heathPreviewTable;

    public HealthPreviewTable(EditorWindow hostWindow)
    {
        _healthPreivewData = new List<object>();
        _heathPreviewTable = new TableView(hostWindow, typeof(HealthPreviewData));

        UpdateTableTitle();
        RefreshData();
    }


    private void UpdateTableTitle()
    {
        _heathPreviewTable.ClearColumns();
        _heathPreviewTable.AddColumn("Time", "Time", 0.2f, TextAnchor.MiddleLeft, "");
        _heathPreviewTable.AddColumn("PassPercent", "PassPercent", 0.4f, TextAnchor.MiddleLeft, "");
        _heathPreviewTable.AddColumn("RootPath", "RootPath", 0.4f, TextAnchor.MiddleLeft, "");


    }

    public void RefreshData(bool forceRefresh = true)
    {
        _healthPreivewData.Add(new HealthPreviewData());
        _heathPreviewTable.RefreshData(_healthPreivewData);
    }

    public void Draw(Rect rect, KeyCode keyCode)
    {
        GUILayout.BeginVertical();
        {
            float startY = TableConst.TableBorder;
            float height = rect.height - startY - 5;
            _heathPreviewTable.Draw(new Rect(TableConst.TableBorder, startY, rect.width * TableConst.SplitterRatio - 1.5f * TableConst.TableBorder, height), false);
        }
        GUILayout.EndVertical();

        EditorGUILayout.HelpBox(new GUIContent("dfdfd").text, MessageType.Info);
    }

}
