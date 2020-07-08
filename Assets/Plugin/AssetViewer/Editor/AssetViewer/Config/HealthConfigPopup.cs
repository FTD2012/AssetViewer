using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HealthConfigPopup : PopupWindowContent
{
    public static string[] s_healthConfigs;
    private static int s_CurrentMode = -1;

    public static int s_currentMode
    {
        get
        {
            if (s_CurrentMode < 0)
                s_CurrentMode = EditorPrefs.GetInt("HealthConfigMode", 0);
            return s_CurrentMode;
        }
        set
        {
            s_CurrentMode = value;
            EditorPrefs.SetInt("HealthConfigMode", value);
        }
    }

    public HealthConfigPopup()
    {
        
    }

    public override void OnGUI(Rect rect)
    {
        Draw(editorWindow, rect.width);

        // Use mouse move so we get hover state correctly in the menu item rows
        if (Event.current.type == EventType.MouseMove)
            Event.current.Use();

        // Escape closes the window
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
        {
            editorWindow.Close();
            GUIUtility.ExitGUI();
        }
    }

    private void Draw(EditorWindow caller, float listElementWidth)
    {
        var drawPos = new Rect(0, 0, listElementWidth, 16);

        // Generic platform modes
        for (var i = 0; i < s_healthConfigs.Length; ++i)
        {
            DoOneMode(drawPos, i);
            drawPos.y += 16;
        }
    }

    void DoOneMode(Rect rect, int index)
    {
        EditorGUI.BeginChangeCheck();
        GUI.Toggle(rect, s_currentMode == index, new GUIContent(s_healthConfigs[index]), "MenuItem");
        if (EditorGUI.EndChangeCheck())
        {
            s_currentMode = index;
            editorWindow.Close();
            GUIUtility.ExitGUI();
        }
    }

    public override Vector2 GetWindowSize()
    {
        var windowSize = new Vector2(100, s_healthConfigs.Length * 16);
        return windowSize;
    }


}
