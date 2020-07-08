using EditorCommon;
using System;
using UnityEditor;
using UnityEngine;

namespace AssetViewer
{
    public enum TextureViewerMode
    {
        ReadWrite = 0,
        MipMap,
        FilterMode,
        Type,
        Resolution,
        WidthVSHeight,
        StandaloneFormat,
        AndroidFormat,
        iOSFormat
    }

    public class TextureViewer : Viewer<TextureViewerData, TextureInfo, TextureViewerModeManager, TextureHealthInfoManager>
    {
        public TextureViewer(EditorWindow hostWindow) : base(hostWindow)
        {

        }
    }

    public class TextureViewerModeManager : ViewerModeManager
    {
        public override string[] GetMode()
        {
            return EditorTool.RemoveAt(Enum.GetNames(typeof(TextureViewerMode)), 5);
        }

        public override ColumnType[] GetDataTable(string textureViewerMode)
        {
            TextureViewerMode textureViewerModeEnum =  (TextureViewerMode)Enum.Parse(typeof(TextureViewerMode), textureViewerMode);
            switch (textureViewerModeEnum)
            {
                case TextureViewerMode.ReadWrite:
                    return new ColumnType[] {
                        new ColumnType("ReadWriteEnable", "R/W Enable", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case TextureViewerMode.MipMap:
                    return new ColumnType[] {
                        new ColumnType("MipmapEnable", "MipmapEnable", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case TextureViewerMode.Type:
                    return new ColumnType[] {
                        new ColumnType("ImportType", "TextureType", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case TextureViewerMode.Resolution:
                    return new ColumnType[] {
                        new ColumnType("SizeStr", "Resolution Range", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case TextureViewerMode.WidthVSHeight:
                    return new ColumnType[] {
                        new ColumnType("WidthAndHeight", "Width VS Height", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case TextureViewerMode.StandaloneFormat:
                    return new ColumnType[] {
                        new ColumnType("StandaloneFormat", "StandaloneFormat", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case TextureViewerMode.AndroidFormat:
                    return new ColumnType[] {
                        new ColumnType("AndroidFormat", "AndroidFormat", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case TextureViewerMode.iOSFormat:
                    return new ColumnType[] {
                        new ColumnType("IosFormat", "iOSFormat", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case TextureViewerMode.FilterMode:
                    return new ColumnType[] {
                        new ColumnType("FilterMode", "FilterMode", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                default:
                    throw new NotImplementedException();
            }
        }

        public override ColumnType[] GetShowTable(string textureViewerMode)
        {
            TextureViewerMode textureViewerModeEnum = (TextureViewerMode)Enum.Parse(typeof(TextureViewerMode), textureViewerMode);
            switch (textureViewerModeEnum)
            {
                case TextureViewerMode.ReadWrite:
                case TextureViewerMode.MipMap:
                case TextureViewerMode.Type:
                case TextureViewerMode.Resolution:
                case TextureViewerMode.WidthVSHeight:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("MemSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>") };
                case TextureViewerMode.StandaloneFormat:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.4f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("StandaloneFormat", "Format", 0.2f, TextAnchor.MiddleCenter, "="),
                        new ColumnType("StandaloneOverriden", "Overriden", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("StandaloneSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case TextureViewerMode.AndroidFormat:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.4f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("AndroidFormat", "Format", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("AndroidOverriden", "Overriden", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("AndroidSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case TextureViewerMode.iOSFormat:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.4f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("IosFormat", "Format", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("IosOverriden", "Overriden", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("IosSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case TextureViewerMode.FilterMode:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.6f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("FilterMode", "FilterMode", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("MemSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                default:
                    throw new NotImplementedException();
            }
        }
    }

    public class TextureHealthInfoManager : HealthInfoManager
    {

    }


}