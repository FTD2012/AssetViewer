using UnityEditor;
using UnityEngine;
using EditorCommon;
using System;
using System.Collections.Generic;

namespace AssetViewer
{
    enum OverviewWinType
    {
        Texture,
        Model,
        Particle,
        Shader
    }

    public class AssetViewerWin : EditorWindow
    {
        private TextureOverviewViewer _textureViewerr;
        private ModelOverviewViewer _modelViewer;
        private ParticleOverviewViewer _particleViewer;
        private ShaderOverviewViewer _shaderViewer;
        private OverviewWinType _currentMode = OverviewWinType.Texture;
        private KeyCode _pressedKey;

        [MenuItem("Window/AssetViewer " + EditorHotkeys.Ctrl_ + "X")]
        static void Create()
        {
            AssetViewerWin resourceInfoWin = GetWindow<AssetViewerWin>();
            resourceInfoWin.minSize = new Vector2(800, 450);
            resourceInfoWin.titleContent = new GUIContent("AssetViewer");    /// TODO: ljm >>> refractor load meathod
        }

        void OnEnable()
        {
            _textureViewerr = new TextureOverviewViewer(this);
            _modelViewer = new ModelOverviewViewer(this);
            _particleViewer = new ParticleOverviewViewer(this);
            _shaderViewer = new ShaderOverviewViewer(this);

            InitHealthData();
        }


        void InitHealthData()
        {
            HealthConfig.Instance().Init();
            HealthConfigPopup.s_healthConfigs = HealthConfig.Instance().GetHealthConfigName();
            LoadHealthConfig();
        }

        void LoadHealthConfig()
        {
            string configName = HealthConfigPopup.s_healthConfigs[HealthConfigPopup.s_currentMode];
            HealthConfig.ConfigJson configJson = HealthConfig.Instance().GetConfig(configName);

            // Texture
            OverviewTableConst.GetSingletonInstance<TextureHealthInfoManager>().Clear();
            foreach (string mode in OverviewTableConst.GetSingletonInstance<TextureOverviewModeManager>().GetMode())
            {
                HealthConfig.ModeConfig modeConfig = configJson.GetModeConfig("Texture", mode);
                if (modeConfig != null)
                {
                    OverviewTableConst.GetSingletonInstance<TextureHealthInfoManager>().AddHealthInfo(mode, modeConfig.Enable, modeConfig.Tip, modeConfig.ConfigValue, modeConfig.Condition);
                }
            }

            // Model
            OverviewTableConst.GetSingletonInstance<ModelHealthInfoManager>().Clear();
            foreach (string mode in OverviewTableConst.GetSingletonInstance<ModelOverviewModeManager>().GetMode())
            {
                HealthConfig.ModeConfig modeConfig = configJson.GetModeConfig("Model", mode);
                if (modeConfig != null)
                {
                    OverviewTableConst.GetSingletonInstance<ModelHealthInfoManager>().AddHealthInfo(mode, modeConfig.Enable, modeConfig.Tip, modeConfig.ConfigValue, modeConfig.Condition);
                }
            }

            // Particle
            OverviewTableConst.GetSingletonInstance<ParticleHealthInfoManager>().Clear();
            foreach (string mode in OverviewTableConst.GetSingletonInstance<ParticleOverviewModeManager>().GetMode())
            {
                HealthConfig.ModeConfig modeConfig = configJson.GetModeConfig("Particle", mode);
                if (modeConfig != null)
                {
                    OverviewTableConst.GetSingletonInstance<ParticleHealthInfoManager>().AddHealthInfo(mode, modeConfig.Enable, modeConfig.Tip, modeConfig.ConfigValue, modeConfig.Condition);
                }
            }

            // Shader
            OverviewTableConst.GetSingletonInstance<ShaderHealthInfoManager>().Clear();
            foreach (string mode in OverviewTableConst.GetSingletonInstance<ShaderOverviewModeManager>().GetMode())
            {
                HealthConfig.ModeConfig modeConfig = configJson.GetModeConfig("Shader", mode);
                if (modeConfig != null)
                {
                    OverviewTableConst.GetSingletonInstance<ShaderHealthInfoManager>().AddHealthInfo(mode, modeConfig.Enable, modeConfig.Tip, modeConfig.ConfigValue, modeConfig.Condition);
                }
            }
        }


        void Update()
        {
            if (Input.GetKeyUp(KeyCode.F5) || Input.GetKeyDown(KeyCode.F5)) // I don't know why this not working.
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

        void OnDestroy()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}