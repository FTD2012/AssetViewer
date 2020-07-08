using EditorCommon;
using System;
using UnityEditor;
using UnityEngine;

namespace AssetViewer
{
    public enum ParticleViewerMode
    {
        MaxParticle = 0,
        Duration,
        PlayOnAwake,
        Looping
    }

    public class ParticleViewer : Viewer<ParticleViewerData, ParticleInfo, ParticleViewerModeManager, ParticleHealthInfoManager>
    {
        public ParticleViewer(EditorWindow hostWindow) : base(hostWindow)
        {

        }
    }

    public class ParticleViewerModeManager : ViewerModeManager
    {
        public override string[] GetMode()
        {
            return Enum.GetNames(typeof(ParticleViewerMode));
        }

        public override ColumnType[] GetDataTable(string particleViewerMode)
        {
            ParticleViewerMode pariticleViewerModeEnum = (ParticleViewerMode)Enum.Parse(typeof(ParticleViewerMode), particleViewerMode);
            switch (pariticleViewerModeEnum)
            {
                case ParticleViewerMode.MaxParticle:
                    return new ColumnType[] {
                        new ColumnType("SizeStr", "MaxParticle", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ParticleViewerMode.Duration:
                    return new ColumnType[] {
                        new ColumnType("DurtationStr", "Duration", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ParticleViewerMode.PlayOnAwake:
                    return new ColumnType[] {
                        new ColumnType("PlayOnAwake", "PlayOnAwake", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ParticleViewerMode.Looping:
                    return new ColumnType[] {
                        new ColumnType("Looping", "Looping", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};

                default:
                    throw new NotImplementedException();
            }
        }

        public override ColumnType[] GetShowTable(string particleViewerMode)
        {
            ParticleViewerMode particleViewerModeEnum = (ParticleViewerMode)Enum.Parse(typeof(ParticleViewerMode), particleViewerMode);
            switch (particleViewerModeEnum)
            {
                case ParticleViewerMode.MaxParticle:
                    return new ColumnType[] {
                        new ColumnType("RealPath", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("MaxParticles", "MaxParticle", 0.2f, TextAnchor.MiddleCenter, "")};
                case ParticleViewerMode.Duration:
                    return new ColumnType[] {
                        new ColumnType("RealPath", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("Duration", "Duration", 0.2f, TextAnchor.MiddleCenter, "")};
                case ParticleViewerMode.PlayOnAwake:
                    return new ColumnType[] {
                        new ColumnType("RealPath", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("PlayOnAwake", "PlayOnAwake", 0.2f, TextAnchor.MiddleCenter, "")};
                case ParticleViewerMode.Looping:
                    return new ColumnType[] {
                        new ColumnType("RealPath", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("Looping", "Looping", 0.2f, TextAnchor.MiddleCenter, "")};
                default:
                    throw new NotImplementedException();
            }
        }
    }

    public class ParticleHealthInfoManager : HealthInfoManager
    {

    }
}