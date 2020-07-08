using UnityEditor;
using UnityEngine;
using EditorCommon;
using System;

namespace AssetViewer
{
    public enum ViewerType
    {
        Texture,
        Model,
        Particle,
        Shader,
        Audio
    }

    public class AssetViewerWin : EditorWindow
    {
        private TextureViewer _textureViewerr;
        private ModelViewer _modelViewer;
        private ParticleViewer _particleViewer;
        private ShaderViewer _shaderViewer;
        private AudioViewer _audioViewer;
        private ViewerType _currentMode = ViewerType.Texture;
        private KeyCode _pressedKey;

        [MenuItem("Window/AssetViewer " + EditorHotkeys.Ctrl_ + "X")]
        private static void Create()
        {
            AssetViewerWin resourceInfoWin = GetWindow<AssetViewerWin>();
            resourceInfoWin.minSize = new Vector2(800, 450);
            resourceInfoWin.titleContent = new GUIContent("AssetViewer");
        }

        private void OnEnable()
        {
            _textureViewerr = new TextureViewer(this);
            _modelViewer = new ModelViewer(this);
            _particleViewer = new ParticleViewer(this);
            _shaderViewer = new ShaderViewer(this);
            _audioViewer = new AudioViewer(this);

            InitHealthData();
        }

        private void InitHealthData()
        {
            HealthConfig.Instance().Init();
            HealthConfigPopup.s_healthConfigs = HealthConfig.Instance().GetHealthConfigName();
            LoadHealthConfig();
        }

        private void LoadHealthConfig()
        {
            string configName = HealthConfigPopup.s_healthConfigs[HealthConfigPopup.s_currentMode];
            HealthConfig.ConfigJson configJson = HealthConfig.Instance().GetConfig(configName);

            // ------------------------------------------------Texture------------------------------------------------
            ViewerConst.GetSingletonInstance<TextureHealthInfoManager>().Clear();
            foreach (string mode in ViewerConst.GetSingletonInstance<TextureViewerModeManager>().GetMode())
            {
                HealthConfig.ModeConfig modeConfig = configJson.GetModeConfig("Texture", mode);
                if (modeConfig != null)
                {
                    ViewerConst.GetSingletonInstance<TextureHealthInfoManager>().AddHealthInfo(mode, modeConfig.Enable, modeConfig.Tip, modeConfig.ConfigValue, modeConfig.Condition);
                }
            }

            // ------------------------------------------------Model------------------------------------------------
            ViewerConst.GetSingletonInstance<ModelHealthInfoManager>().Clear();
            foreach (string mode in ViewerConst.GetSingletonInstance<ModelViewerModeManager>().GetMode())
            {
                HealthConfig.ModeConfig modeConfig = configJson.GetModeConfig("Model", mode);
                if (modeConfig != null)
                {
                    ViewerConst.GetSingletonInstance<ModelHealthInfoManager>().AddHealthInfo(mode, modeConfig.Enable, modeConfig.Tip, modeConfig.ConfigValue, modeConfig.Condition);
                }
            }

            // ------------------------------------------------Particle------------------------------------------------
            ViewerConst.GetSingletonInstance<ParticleHealthInfoManager>().Clear();
            foreach (string mode in ViewerConst.GetSingletonInstance<ParticleViewerModeManager>().GetMode())
            {
                HealthConfig.ModeConfig modeConfig = configJson.GetModeConfig("Particle", mode);
                if (modeConfig != null)
                {
                    ViewerConst.GetSingletonInstance<ParticleHealthInfoManager>().AddHealthInfo(mode, modeConfig.Enable, modeConfig.Tip, modeConfig.ConfigValue, modeConfig.Condition);
                }
            }

            // ------------------------------------------------Shader------------------------------------------------
            ViewerConst.GetSingletonInstance<ShaderHealthInfoManager>().Clear();
            foreach (string mode in ViewerConst.GetSingletonInstance<ShaderViewerModeManager>().GetMode())
            {
                HealthConfig.ModeConfig modeConfig = configJson.GetModeConfig("Shader", mode);
                if (modeConfig != null)
                {
                    ViewerConst.GetSingletonInstance<ShaderHealthInfoManager>().AddHealthInfo(mode, modeConfig.Enable, modeConfig.Tip, modeConfig.ConfigValue, modeConfig.Condition);
                }
            }

            // ------------------------------------------------Audio------------------------------------------------
            ViewerConst.GetSingletonInstance<AudioHealthInfoManager>().Clear();
            foreach (string mode in ViewerConst.GetSingletonInstance<AudioViewerModeManager>().GetMode())
            {
                HealthConfig.ModeConfig modeConfig = configJson.GetModeConfig("Audio", mode);
                if (modeConfig != null)
                {
                    ViewerConst.GetSingletonInstance<AudioHealthInfoManager>().AddHealthInfo(mode, modeConfig.Enable, modeConfig.Tip, modeConfig.ConfigValue, modeConfig.Condition);
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.F5) || Input.GetKeyDown(KeyCode.F5)) // I don't know why this not working.
            {
                _pressedKey = KeyCode.F5;
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal(TableStyles.Toolbar);
            {
                _currentMode = (ViewerType)GUILayout.SelectionGrid((int)_currentMode, Enum.GetNames(typeof(ViewerType)), Enum.GetNames(typeof(ViewerType)).Length, TableStyles.ToolbarButton);
            }
            GUILayout.EndHorizontal();

            Rect viewRect = new Rect(0, TableConst.TopBarHeight, position.width, position.height - TableConst.TopBarHeight);
            if (_currentMode == ViewerType.Texture)
            {
                _textureViewerr.Draw(viewRect, _pressedKey);
            }
            else if (_currentMode == ViewerType.Model)
            {
                _modelViewer.Draw(viewRect, _pressedKey);
            }
            else if (_currentMode == ViewerType.Particle)
            {
                _particleViewer.Draw(viewRect, _pressedKey);
            }
            else if (_currentMode == ViewerType.Shader)
            {
                _shaderViewer.Draw(viewRect, _pressedKey);
            }
            else if (_currentMode == ViewerType.Audio)
            {
                _audioViewer.Draw(viewRect, _pressedKey);
            }
            _pressedKey = KeyCode.None;
        }

        private void OnDestroy()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}