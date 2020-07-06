using UnityEditor;
using UnityEngine;
using EditorCommon;
using System;
using System.Collections.Generic;

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
            resourceInfoWin.minSize = new Vector2(800, 450);
            resourceInfoWin.titleContent = new GUIContent("ResViewer", (Texture)Resources.Load("icon1"));    /// TODO: ljm >>> refractor load meathod
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
            OverviewTableConst.GetSingletonInstance<TextureHealthInfoManager>().AddHealthInfo(TextureOverviewMode.ReadWrite.ToString(), true, OverviewTableString.Texture_ReadWrite, configJson.Texture.ReadWrite, new List<object> { true });
            OverviewTableConst.GetSingletonInstance<TextureHealthInfoManager>().AddHealthInfo(TextureOverviewMode.MipMap.ToString(), true, OverviewTableString.Texture_MipMap, configJson.Texture.MipMap, new List<object> { true });
            OverviewTableConst.GetSingletonInstance<TextureHealthInfoManager>().AddHealthInfo(TextureOverviewMode.Type.ToString(), false, OverviewTableString.Texture_Type, 0, null);
            OverviewTableConst.GetSingletonInstance<TextureHealthInfoManager>().AddHealthInfo(TextureOverviewMode.Resolution.ToString(), true, OverviewTableString.Texture_Resolution, configJson.Texture.ResolutionCount, new List<object> { configJson.Texture.Resolution });
            OverviewTableConst.GetSingletonInstance<TextureHealthInfoManager>().AddHealthInfo(TextureOverviewMode.StandaloneFormat.ToString(), false, OverviewTableString.Texture_Standalone, 0, null);
            OverviewTableConst.GetSingletonInstance<TextureHealthInfoManager>().AddHealthInfo(TextureOverviewMode.AndroidFormat.ToString(), false, OverviewTableString.Texture_Android, 0, null);
            OverviewTableConst.GetSingletonInstance<TextureHealthInfoManager>().AddHealthInfo(TextureOverviewMode.iOSFormat.ToString(), false, OverviewTableString.Texture_iOS, 0, null);
            OverviewTableConst.GetSingletonInstance<TextureHealthInfoManager>().AddHealthInfo(TextureOverviewMode.FilterMode.ToString(), true, OverviewTableString.Texture_FilterMode, configJson.Texture.Trilinear, new List<object> { FilterMode.Trilinear });

            // Particle
            OverviewTableConst.GetSingletonInstance<ParticleHealthInfoManager>().Clear();
            OverviewTableConst.GetSingletonInstance<ParticleHealthInfoManager>().AddHealthInfo(ParticleOverviewMode.MaxParticle.ToString(), true, string.Format(OverviewTableString.Particle_Max, configJson.Particle.MaxParticle), configJson.Particle.MaxParticleCount, new List<object> { configJson.Particle.MaxParticle });
            OverviewTableConst.GetSingletonInstance<ParticleHealthInfoManager>().AddHealthInfo(ParticleOverviewMode.Duration.ToString(), false, OverviewTableString.Particle_Duration, 0, null);
            OverviewTableConst.GetSingletonInstance<ParticleHealthInfoManager>().AddHealthInfo(ParticleOverviewMode.PlayOnAwake.ToString(), false, OverviewTableString.Texture_FilterMode, configJson.Texture.Trilinear, new List<object> { FilterMode.Trilinear });
            OverviewTableConst.GetSingletonInstance<ParticleHealthInfoManager>().AddHealthInfo(ParticleOverviewMode.Looping.ToString(), false, string.Format(OverviewTableString.Texture_FilterMode, configJson.Texture.Trilinear), configJson.Texture.Trilinear, new List<object> { FilterMode.Trilinear });

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
    }
}