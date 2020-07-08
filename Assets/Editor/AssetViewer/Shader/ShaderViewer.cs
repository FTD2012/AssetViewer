using EditorCommon;
using System;
using UnityEditor;
using UnityEngine;

namespace AssetViewer
{
    public enum ShaderViewerMode
    {
        Sample = 0,
        Pass,
        Instruction,
        RenderType,
        Variant,
        RenderQueue,
        MaxLOD,
        Property,
        SubShader
    }

    public class ShaderViewer : Viewer<ShaderViewerData, ShaderInfo, ShaderViewerModeManager, ShaderHealthInfoManager>
    {
        public ShaderViewer(EditorWindow hostWindow) : base(hostWindow)
        {

        }
    }

    public class ShaderViewerModeManager : ViewerModeManager
    {
        public override string[] GetMode()
        {
            return Enum.GetNames(typeof(ShaderViewerMode));
        }

        public override ColumnType[] GetDataTable(string shaderViewerMode)
        {
            ShaderViewerMode shaderViewerModeEnum = (ShaderViewerMode)Enum.Parse(typeof(ShaderViewerMode), shaderViewerMode);
            switch (shaderViewerModeEnum)
            {
                case ShaderViewerMode.MaxLOD:
                    return new ColumnType[] {
                        new ColumnType("MaxLODStr", "MaxLOD", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ShaderViewerMode.RenderQueue:
                    return new ColumnType[] {
                        new ColumnType("RenderQueueStr", "RenderQueue", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ShaderViewerMode.Pass:
                    return new ColumnType[] {
                        new ColumnType("Pass", "Pass", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ShaderViewerMode.Instruction:
                    return new ColumnType[] {
                        new ColumnType("InstructionStr", "Instruction", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ShaderViewerMode.Variant:
                    return new ColumnType[] {
                        new ColumnType("Variant", "Variant", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ShaderViewerMode.Property:
                    return new ColumnType[] {
                        new ColumnType("Property", "Property", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ShaderViewerMode.SubShader:
                    return new ColumnType[] {
                        new ColumnType("SubShader", "SubShader", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ShaderViewerMode.Sample:
                    return new ColumnType[] {
                        new ColumnType("Sample", "Sample", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ShaderViewerMode.RenderType:
                    return new ColumnType[] {
                        new ColumnType("RenderType", "RenderType", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                default:
                    throw new NotImplementedException();
            }
        }

        public override ColumnType[] GetShowTable(string shaderViewerMode)
        {
            ShaderViewerMode shaderViewerModeEnum = (ShaderViewerMode)Enum.Parse(typeof(ShaderViewerMode), shaderViewerMode);
            switch (shaderViewerModeEnum)
            {
                case ShaderViewerMode.MaxLOD:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("MaxLOD", "MaxLOD", 0.2f, TextAnchor.MiddleCenter, "")};
                case ShaderViewerMode.RenderQueue:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("RenderQueue", "RenderQueue", 0.2f, TextAnchor.MiddleCenter, "")};
                case ShaderViewerMode.Pass:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("Pass", "Pass", 0.2f, TextAnchor.MiddleCenter, "")};
                case ShaderViewerMode.Instruction:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("Instruction", "Instruction", 0.2f, TextAnchor.MiddleCenter, "")};
                case ShaderViewerMode.Variant:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("Variant", "Variant", 0.2f, TextAnchor.MiddleCenter, "")};
                case ShaderViewerMode.Property:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("Property", "Property", 0.2f, TextAnchor.MiddleCenter, "")};
                case ShaderViewerMode.SubShader:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("SubShader", "SubShader", 0.2f, TextAnchor.MiddleCenter, "")};
                case ShaderViewerMode.Sample:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("Sample", "Sample", 0.2f, TextAnchor.MiddleCenter, "")};
                case ShaderViewerMode.RenderType:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("RenderType", "RenderType", 0.2f, TextAnchor.MiddleCenter, "")};
                default:
                    throw new NotImplementedException();
            }
        }

    }


    public class ShaderHealthInfoManager : HealthInfoManager
    {

    }

}