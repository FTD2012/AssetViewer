using EditorCommon;
using System;
using UnityEditor;
using UnityEngine;

namespace AssetViewer
{
    public enum ShaderOverviewMode
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

    public class ShaderOverviewViewer : OverviewViewer<ShaderOverviewData, ShaderInfo, ShaderOverviewModeManager, ShaderHealthInfoManager>
    {
        public ShaderOverviewViewer(EditorWindow hostWindow) : base(hostWindow)
        {

        }
    }

    public class ShaderOverviewModeManager : OverviewModeManager
    {
        public override string[] GetMode()
        {
            return Enum.GetNames(typeof(ShaderOverviewMode));
        }

        public override ColumnType[] GetDataTable(string shaderOverviewMode)
        {
            ShaderOverviewMode shaderOverviewModeEnum = (ShaderOverviewMode)Enum.Parse(typeof(ShaderOverviewMode), shaderOverviewMode);
            switch (shaderOverviewModeEnum)
            {
                case ShaderOverviewMode.MaxLOD:
                    return new ColumnType[] {
                        new ColumnType("MaxLODStr", "MaxLOD", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ShaderOverviewMode.RenderQueue:
                    return new ColumnType[] {
                        new ColumnType("RenderQueueStr", "RenderQueue", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ShaderOverviewMode.Pass:
                    return new ColumnType[] {
                        new ColumnType("Pass", "Pass", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ShaderOverviewMode.Instruction:
                    return new ColumnType[] {
                        new ColumnType("InstructionStr", "Instruction", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ShaderOverviewMode.Variant:
                    return new ColumnType[] {
                        new ColumnType("Variant", "Variant", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ShaderOverviewMode.Property:
                    return new ColumnType[] {
                        new ColumnType("Property", "Property", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ShaderOverviewMode.SubShader:
                    return new ColumnType[] {
                        new ColumnType("SubShader", "SubShader", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ShaderOverviewMode.Sample:
                    return new ColumnType[] {
                        new ColumnType("Sample", "Sample", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ShaderOverviewMode.RenderType:
                    return new ColumnType[] {
                        new ColumnType("RenderType", "RenderType", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                default:
                    throw new NotImplementedException();
            }
        }

        public override ColumnType[] GetShowTable(string shaderOverviewMode)
        {
            ShaderOverviewMode shaderOverviewModeEnum = (ShaderOverviewMode)Enum.Parse(typeof(ShaderOverviewMode), shaderOverviewMode);
            switch (shaderOverviewModeEnum)
            {
                case ShaderOverviewMode.MaxLOD:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("MaxLOD", "MaxLOD", 0.2f, TextAnchor.MiddleCenter, "")};
                case ShaderOverviewMode.RenderQueue:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("RenderQueue", "RenderQueue", 0.2f, TextAnchor.MiddleCenter, "")};
                case ShaderOverviewMode.Pass:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("Pass", "Pass", 0.2f, TextAnchor.MiddleCenter, "")};
                case ShaderOverviewMode.Instruction:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("Instruction", "Instruction", 0.2f, TextAnchor.MiddleCenter, "")};
                case ShaderOverviewMode.Variant:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("Variant", "Variant", 0.2f, TextAnchor.MiddleCenter, "")};
                case ShaderOverviewMode.Property:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("Property", "Property", 0.2f, TextAnchor.MiddleCenter, "")};
                case ShaderOverviewMode.SubShader:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("SubShader", "SubShader", 0.2f, TextAnchor.MiddleCenter, "")};
                case ShaderOverviewMode.Sample:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.8f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("Sample", "Sample", 0.2f, TextAnchor.MiddleCenter, "")};
                case ShaderOverviewMode.RenderType:
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