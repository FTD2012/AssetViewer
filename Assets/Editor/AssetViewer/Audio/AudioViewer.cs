using EditorCommon;
using System;
using UnityEditor;
using UnityEngine;

namespace AssetViewer
{
    public enum AudioOverviewMode
    {
        MONO = 0,
        LoadInBackground,
        Ambisonic,
        AndroidLoadType,
        iOSLoadType,
        AndroidCompressionFormat,
        iOSCompressionFormat
    }

    public class AudioOverviewViewer : OverviewViewer<AudioOverviewData, AudioInfo, AudioOverviewModeManager, AudioHealthInfoManager>
    {
        public AudioOverviewViewer(EditorWindow hostWindow) : base(hostWindow)
        {

        }
    }

    public class AudioOverviewModeManager : OverviewModeManager
    {
        public override string[] GetMode()
        {
            return Enum.GetNames(typeof(AudioOverviewMode));
        }

        public override ColumnType[] GetDataTable(string audioOverviewMode)
        {
            AudioOverviewMode audioOverviewModeEnum = (AudioOverviewMode)Enum.Parse(typeof(AudioOverviewMode), audioOverviewMode);
            switch (audioOverviewModeEnum)
            {
                case AudioOverviewMode.MONO:
                    return new ColumnType[] {
                        new ColumnType("ForceToMono", "Force to Mono", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case AudioOverviewMode.LoadInBackground:
                    return new ColumnType[] {
                        new ColumnType("LoadInBackground", "Load in Background", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case AudioOverviewMode.Ambisonic:
                    return new ColumnType[] {
                        new ColumnType("Ambisonic", "Ambisonic", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case AudioOverviewMode.AndroidLoadType:
                    return new ColumnType[] {
                        new ColumnType("AndroidAudioClipLoadType", "AndroidLoadType", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case AudioOverviewMode.iOSLoadType:
                    return new ColumnType[] {
                        new ColumnType("iOSAudioClipLoadType", "iOSLoadType", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case AudioOverviewMode.AndroidCompressionFormat:
                    return new ColumnType[] {
                        new ColumnType("AndroidAudioCompressionFormat", "AndroidCompressionFormat", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case AudioOverviewMode.iOSCompressionFormat:
                    return new ColumnType[] {
                        new ColumnType("iOSAudioCompressionFormat", "iOSCompressionFormat", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                default:
                    throw new NotImplementedException();
            }
        }

        public override ColumnType[] GetShowTable(string audioOverviewMode)
        {
            AudioOverviewMode audioOverviewModeEnum = (AudioOverviewMode)Enum.Parse(typeof(AudioOverviewMode), audioOverviewMode);
            switch (audioOverviewModeEnum)
            {
                case AudioOverviewMode.MONO:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.6f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("ForceToMono", "Force to Mono", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("MemSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>") };
                case AudioOverviewMode.LoadInBackground:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.6f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("LoadInBackground", "Load in Background", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("MemSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>") };
                case AudioOverviewMode.Ambisonic:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.6f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("Ambisonic", "Ambisonic", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("MemSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>") };
                case AudioOverviewMode.AndroidLoadType:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.6f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("AndroidAudioClipLoadType", "AndroidLoadType", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("MemSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>") };
                case AudioOverviewMode.iOSLoadType:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.6f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("iOSAudioClipLoadType", "iOSLoadType", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("MemSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>") };
                case AudioOverviewMode.AndroidCompressionFormat:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.6f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("AndroidAudioCompressionFormat", "AndroidCompressionFormat", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("MemSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>") };
                case AudioOverviewMode.iOSCompressionFormat:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.6f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("iOSAudioCompressionFormat", "iOSCompressionFormat", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("MemSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>") };
                default:
                    throw new NotImplementedException();
            }
        }
    }

    public class AudioHealthInfoManager : HealthInfoManager
    {

    }
}