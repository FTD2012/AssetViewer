using UnityEditor;
using UnityEngine;
using EditorCommon;

namespace ResourceFormat
{
    enum OverviewWinType
    {
        Texture,
        Model,
    }

    public class ResourceOverviewWin : EditorWindow
    {
        private TextureOverviewViewer _textureViewerr;
        private ModelOverviewViewer _modelViewer;
        private OverviewWinType _currentMode = OverviewWinType.Texture;
        private KeyCode _pressedKey;

        [MenuItem("UComponents/ResourceOverviewWin " + EditorHotkeys.Ctrl_ + "X")]
        static void Create()
        {
            ResourceOverviewWin resourceInfoWin = GetWindow<ResourceOverviewWin>();
            resourceInfoWin.minSize = new Vector2(400, 300);
            resourceInfoWin.titleContent = new GUIContent("ResViewer", (Texture)Resources.Load("icon1"));    /// TODO: ljm >>> refractor load meathod
                                                                                                                    /// TODO: ljm >>> reset texture
        }

        void OnEnable()
        {
            _textureViewerr = new TextureOverviewViewer(this);
            _modelViewer = new ModelOverviewViewer(this);
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.F5)) // I don't know why this not working.
            {
                _pressedKey = KeyCode.F5;
            }
        }

        void OnGUI()
        {
            GUILayout.BeginHorizontal(TableStyles.Toolbar);
            {
                _currentMode = (OverviewWinType)GUILayout.SelectionGrid((int)_currentMode, OverviewTableConst.OverviewModeName, OverviewTableConst.OverviewModeName.Length, TableStyles.ToolbarButton);
            }
            GUILayout.EndHorizontal();

            Rect viewRect = new Rect(0, TableConst.TopBarHeight, position.width, position.height - TableConst.TopBarHeight);
            if (_currentMode == OverviewWinType.Texture)
            {
                _textureViewerr.Draw(viewRect, _pressedKey);
            }
            else if (_currentMode == OverviewWinType.Model)
            {
                _modelViewer.Draw(viewRect);
            }

            _pressedKey = KeyCode.None;
        }
    }
}