using UnityEditor;
using UnityEngine;
using EditorCommon;
using System;

namespace ResourceFormat
{
    enum OverviewWinType
    {
        Texture,
        Model,
        Particle,
        Shader
    }

    public class ResourceOverviewWin : EditorWindow
    {
        private TextureOverviewViewer _textureViewerr;
        private ModelOverviewViewer _modelViewer;
        private ParticleOverviewViewer _particleViewer;
        private ShaderOverviewViewer _shaderViewer;
        private OverviewWinType _currentMode = OverviewWinType.Texture;
        private KeyCode _pressedKey;

        [MenuItem("UComponents/ResourceOverviewWin " + EditorHotkeys.Ctrl_ + "X")]
        static void Create()
        {
            ResourceOverviewWin resourceInfoWin = GetWindow<ResourceOverviewWin>();
            resourceInfoWin.minSize = new Vector2(400, 300);
            resourceInfoWin.titleContent = new GUIContent("ResViewer", (Texture)Resources.Load("icon1"));    /// TODO: ljm >>> refractor load meathod
        }

        void OnEnable()
        {
            _textureViewerr = new TextureOverviewViewer(this);
            _modelViewer = new ModelOverviewViewer(this);
            _particleViewer = new ParticleOverviewViewer(this);
            _shaderViewer = new ShaderOverviewViewer(this);
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
                _currentMode = (OverviewWinType)GUILayout.SelectionGrid((int)_currentMode, Enum.GetNames(typeof(OverviewWinType)), Enum.GetNames(typeof(OverviewWinType)).Length, TableStyles.ToolbarButton);
            }
            GUILayout.EndHorizontal();

            Rect viewRect = new Rect(0, TableConst.TopBarHeight, position.width, position.height - TableConst.TopBarHeight);
            if (_currentMode == OverviewWinType.Texture)
            {
                _textureViewerr.Draw(viewRect, _pressedKey);
            }
            else if (_currentMode == OverviewWinType.Model)
            {
                _modelViewer.Draw(viewRect, _pressedKey);
            }
            else if (_currentMode == OverviewWinType.Particle)
            {
                _particleViewer.Draw(viewRect, _pressedKey);
            }
            else if (_currentMode == OverviewWinType.Shader)
            {
                _shaderViewer.Draw(viewRect, _pressedKey);
            }
            _pressedKey = KeyCode.None;
        }
    }
}