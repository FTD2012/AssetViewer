using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace ResourceFormat
{
    public enum FormatMode
    {
        Texture,
        Model,
        Animation,
    }

    public class ModeManager
    {
        private FormatMode _currentMode = FormatMode.Texture;
        private Dictionary<FormatMode, BaseViewer> _mode = null;

        public ModeManager(EditorWindow host)
        {
            _currentMode = (FormatMode)EditorPrefs.GetInt(TableConst.CurrentMode);
            _mode = new Dictionary<FormatMode, BaseViewer>()
            {
                { FormatMode.Texture, new TextureViewer(host) },
                { FormatMode.Model, new ModelViewer(host) },
                { FormatMode.Animation, new AnimationViewer(host) },
            };
        }

        private BaseViewer GetCurrentViewer()
        {
            BaseViewer view;
            if (!_mode.TryGetValue(_currentMode, out view))
            {
                return null;
            }
            return view;
        }

        private void SwitchTo(FormatMode newMode)
        {
            if (_currentMode == newMode)
            {
                return;
            }

            BaseViewer preViewer = GetCurrentViewer();
            if (preViewer != null)
            {
                preViewer.OnLeave();
            }

            _currentMode = newMode;

            BaseViewer curViewer = GetCurrentViewer();
            if (curViewer != null)
            {
                curViewer.OnEnter();
            }

            EditorPrefs.SetInt(TableConst.CurrentMode, (int)(_currentMode));
        }

        #region draw


        #endregion draw

        public void OnGUI(Rect rect)
        {
            GUILayout.BeginHorizontal();
            {
                //GUILayout.FlexibleSpace();
                SwitchTo((FormatMode)GUILayout.SelectionGrid((int)_currentMode, TableConst.Modes, TableConst.Modes.Length, TableStyles.ToolbarButton));
                //SwitchTo((FormatMode)GUILayout.Toolbar((int)_currentMode, TableConst.Modes, GUILayout.MaxWidth(400)));
                //GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            BaseViewer viewer = GetCurrentViewer();
            if (viewer != null)
            {
                viewer.Draw(new Rect(0, TableConst.TopBarHeight, rect.width, rect.height - TableConst.TopBarHeight));
            }
            else
            {
                Debug.LogError("Invalid resource format viewer.");
            }
        }
    }

}