using EditorCommon;
using System;
using UnityEditor;
using UnityEngine;

namespace ResourceFormat
{
    public enum ParticleOverviewMode
    {
        MaxParticle = 0,
        Duration,
        PlayOnAwake,
        Looping
    }

    public class ParticleOverviewViewer : OverviewViewer<ParticleOverviewData, ParticleInfo, ParticleOverviewModeManager, ParticleHealthInfoManager>
    {
        public ParticleOverviewViewer(EditorWindow hostWindow) : base(hostWindow)
        {

        }
    }

    public class ParticleOverviewModeManager : OverviewModeManager
    {
        public override string[] GetMode()
        {
            return Enum.GetNames(typeof(ParticleOverviewMode));
        }

        public override ColumnType[] GetDataTable(string particleOverviewMode)
        {
            ParticleOverviewMode textureOverviewModeEnum = (ParticleOverviewMode)Enum.Parse(typeof(ParticleOverviewMode), particleOverviewMode);
            switch (textureOverviewModeEnum)
            {
                case ParticleOverviewMode.MaxParticle:
                    return new ColumnType[] {
                        new ColumnType("SizeStr", "MaxParticle", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ParticleOverviewMode.Duration:
                    return new ColumnType[] {
                        new ColumnType("DurtationStr", "Duration", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ParticleOverviewMode.PlayOnAwake:
                    return new ColumnType[] {
                        new ColumnType("PlayOnAwake", "PlayOnAwake", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ParticleOverviewMode.Looping:
                    return new ColumnType[] {
                        new ColumnType("Looping", "Looping", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};

                default:
                    throw new NotImplementedException();
            }
        }

        public override ColumnType[] GetShowTable(string particleOverviewMode)
        {
            ParticleOverviewMode textureOverviewModeEnum = (ParticleOverviewMode)Enum.Parse(typeof(ParticleOverviewMode), particleOverviewMode);
            switch (textureOverviewModeEnum)
            {
                case ParticleOverviewMode.MaxParticle:
                    return new ColumnType[] {
                        new ColumnType("RealPath", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("MaxParticles", "MaxParticle", 0.2f, TextAnchor.MiddleCenter, "")};
                case ParticleOverviewMode.Duration:
                    return new ColumnType[] {
                        new ColumnType("RealPath", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("Duration", "Duration", 0.2f, TextAnchor.MiddleCenter, "")};
                case ParticleOverviewMode.PlayOnAwake:
                    return new ColumnType[] {
                        new ColumnType("RealPath", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("PlayOnAwake", "PlayOnAwake", 0.2f, TextAnchor.MiddleCenter, "")};
                case ParticleOverviewMode.Looping:
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