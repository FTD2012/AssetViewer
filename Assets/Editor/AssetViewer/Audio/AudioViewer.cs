using EditorCommon;
using System;
using UnityEditor;
using UnityEngine;

namespace AssetViewer
{
    public enum AudioViewerMode
    {
        Size = 0,
        MONO,
        LoadInBackground,
        Ambisonic,
        AndroidLoadType,
        iOSLoadType,
        AndroidCompressionFormat,
        iOSCompressionFormat
    }

    public class AudioViewer : Viewer<AudioViewerData, AudioInfo, AudioViewerModeManager, AudioHealthInfoManager>
    {
        public AudioViewer(EditorWindow hostWindow) : base(hostWindow)
        {

        }
    }

    public class AudioViewerModeManager : ViewerModeManager
    {
        public override string[] GetMode()
        {
            return Enum.GetNames(typeof(AudioViewerMode));
        }

        public override ColumnType[] GetDataTable(string audioViewerMode)
        {
            AudioViewerMode audioViewerModeEnum = (AudioViewerMode)Enum.Parse(typeof(AudioViewerMode), audioViewerMode);
            switch (audioViewerModeEnum)
            {
                case AudioViewerMode.Size:
                    return new ColumnType[] {
                        new ColumnType("All", "All", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("TotalOriginSize", "OriginSize", 0.3f, TextAnchor.MiddleCenter, "<fmt_bytes>"),
                        new ColumnType("TotalCompressedSize", "CompressedSize", 0.3f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case AudioViewerMode.MONO:
                    return new ColumnType[] {
                        new ColumnType("ForceToMono", "Force to Mono", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case AudioViewerMode.LoadInBackground:
                    return new ColumnType[] {
                        new ColumnType("LoadInBackground", "Load in Background", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case AudioViewerMode.Ambisonic:
                    return new ColumnType[] {
                        new ColumnType("Ambisonic", "Ambisonic", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case AudioViewerMode.AndroidLoadType:
                    return new ColumnType[] {
                        new ColumnType("AndroidAudioClipLoadType", "AndroidLoadType", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case AudioViewerMode.iOSLoadType:
                    return new ColumnType[] {
                        new ColumnType("iOSAudioClipLoadType", "iOSLoadType", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case AudioViewerMode.AndroidCompressionFormat:
                    return new ColumnType[] {
                        new ColumnType("AndroidAudioCompressionFormat", "AndroidCompressionFormat", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case AudioViewerMode.iOSCompressionFormat:
                    return new ColumnType[] {
                        new ColumnType("iOSAudioCompressionFormat", "iOSCompressionFormat", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                default:
                    throw new NotImplementedException();
            }
        }

        public override ColumnType[] GetShowTable(string audioViewerMode)
        {
            AudioViewerMode audioViewerModeEnum = (AudioViewerMode)Enum.Parse(typeof(AudioViewerMode), audioViewerMode);
            switch (audioViewerModeEnum)
            {
                case AudioViewerMode.Size:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.6f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("OriginSize", "OriginSize", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>"),
                        new ColumnType("CompressedSize", "CompressedSize", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>") };
                case AudioViewerMode.MONO:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.6f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("ForceToMono", "Force to Mono", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("MemSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>") };
                case AudioViewerMode.LoadInBackground:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.6f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("LoadInBackground", "Load in Background", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("MemSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>") };
                case AudioViewerMode.Ambisonic:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.6f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("Ambisonic", "Ambisonic", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("MemSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>") };
                case AudioViewerMode.AndroidLoadType:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.6f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("AndroidAudioClipLoadType", "AndroidLoadType", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("MemSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>") };
                case AudioViewerMode.iOSLoadType:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.6f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("iOSAudioClipLoadType", "iOSLoadType", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("MemSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>") };
                case AudioViewerMode.AndroidCompressionFormat:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.6f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("AndroidAudioCompressionFormat", "AndroidCompressionFormat", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("MemSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>") };
                case AudioViewerMode.iOSCompressionFormat:
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