using UnityEditor;
using UnityEngine;
using EditorCommon;
using System;
using ResourceFormat;

public class ResourceHealthWin : EditorWindow
{
    protected string _rootPath = string.Empty;

    private HealthPreviewTable _healthPreviewTable;

    private KeyCode _pressedKey;


    [MenuItem("UComponents/ResourceHealthWin " + EditorHotkeys.Alt_ + "X")]
    static void Create()
    {
        ResourceHealthWin healthWin = GetWindow<ResourceHealthWin>();
        healthWin.minSize = new Vector2(400, 300);
        healthWin.titleContent = new GUIContent("HealthViewer", (Texture)Resources.Load("icon1"));    /// TODO: ljm >>> refractor load meathod
    }

    private void OnEnable()
    {
        _healthPreviewTable = new HealthPreviewTable(this);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F5)) // I don't know why this not working.
        {
            _pressedKey = KeyCode.F5;
        }
    }

    private void OnGUI()
    {
        // 1. root path
        GUILayout.BeginHorizontal(TableStyles.Toolbar);
        {
            GUILayout.Label("RootPath: ", GUILayout.Width(100));
            _rootPath = EditorGUILayout.TextField(_rootPath, TableStyles.TextField, GUILayout.Width(360)); /// TODO: ljm >>> change to select path
            if (GUILayout.Button("Refresh Data", TableStyles.ToolbarButton, GUILayout.MaxWidth(120)) || _pressedKey == KeyCode.F5)
            {
            }
        }
        GUILayout.EndHorizontal();
        Rect viewRect = new Rect(0, TableConst.TopBarHeight, position.width, position.height - TableConst.TopBarHeight);
        _healthPreviewTable.Draw(viewRect, _pressedKey);
    }

}
