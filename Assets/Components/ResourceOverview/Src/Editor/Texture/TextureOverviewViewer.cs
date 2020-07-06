using EditorCommon;
using System;
using UnityEditor;
using UnityEngine;

namespace ResourceFormat
{
    public enum TextureOverviewMode
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

    public class TextureOverviewViewer : OverviewViewer<TextureOverviewData, TextureInfo, TextureOverviewModeManager, TextureHealthInfoManager>
    {
        public TextureOverviewViewer(EditorWindow hostWindow) : base(hostWindow)
        {

        }
    }

    public class TextureOverviewModeManager : OverviewModeManager
    {
        public override string[] GetMode()
        {
            return EditorTool.RemoveAt(Enum.GetNames(typeof(TextureOverviewMode)), 5);
        }

        public override ColumnType[] GetDataTable(string textureOverviewMode)
        {
            TextureOverviewMode textureOverviewModeEnum =  (TextureOverviewMode)Enum.Parse(typeof(TextureOverviewMode), textureOverviewMode);
            switch (textureOverviewModeEnum)
            {
                case TextureOverviewMode.ReadWrite:
                    return new ColumnType[] {
                        new ColumnType("ReadWriteEnable", "R/W Enable", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case TextureOverviewMode.MipMap:
                    return new ColumnType[] {
                        new ColumnType("MipmapEnable", "MipmapEnable", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case TextureOverviewMode.Type:
                    return new ColumnType[] {
                        new ColumnType("ImportType", "TextureType", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case TextureOverviewMode.Resolution:
                    return new ColumnType[] {
                        new ColumnType("SizeStr", "Resolution Range", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case TextureOverviewMode.WidthVSHeight:
                    return new ColumnType[] {
                        new ColumnType("WidthAndHeight", "Width VS Height", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case TextureOverviewMode.StandaloneFormat:
                    return new ColumnType[] {
                        new ColumnType("StandaloneFormat", "StandaloneFormat", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case TextureOverviewMode.AndroidFormat:
                    return new ColumnType[] {
                        new ColumnType("AndroidFormat", "AndroidFormat", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case TextureOverviewMode.iOSFormat:
                    return new ColumnType[] {
                        new ColumnType("IosFormat", "iOSFormat", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case TextureOverviewMode.FilterMode:
                    return new ColumnType[] {
                        new ColumnType("FilterMode", "FilterMode", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                default:
                    throw new NotImplementedException();
            }
        }

        public override ColumnType[] GetShowTable(string textureOverviewMode)
        {
            TextureOverviewMode textureOverviewModeEnum = (TextureOverviewMode)Enum.Parse(typeof(TextureOverviewMode), textureOverviewMode);
            switch (textureOverviewModeEnum)
            {
                case TextureOverviewMode.ReadWrite:
                case TextureOverviewMode.MipMap:
                case TextureOverviewMode.Type:
                case TextureOverviewMode.Resolution:
                case TextureOverviewMode.WidthVSHeight:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("MemSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>") };
                case TextureOverviewMode.StandaloneFormat:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.4f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("StandaloneFormat", "Format", 0.2f, TextAnchor.MiddleCenter, "="),
                        new ColumnType("StandaloneOverriden", "Overriden", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("StandaloneSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case TextureOverviewMode.AndroidFormat:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.4f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("AndroidFormat", "Format", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("AndroidOverriden", "Overriden", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("AndroidSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case TextureOverviewMode.iOSFormat:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.4f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("IosFormat", "Format", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("IosOverriden", "Overriden", 0.2f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("IosSize", "Memory", 0.2f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case TextureOverviewMode.FilterMode:
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