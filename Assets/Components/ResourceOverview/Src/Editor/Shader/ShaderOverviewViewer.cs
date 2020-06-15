using EditorCommon;
using System;
using UnityEditor;
using UnityEngine;

namespace ResourceFormat
{
    public enum ShaderOverviewMode
    {
        MaxLOD = 0,
        RenderQueue,
        Pass,
        Instruction,
        Variant,
        Property
    }

    public class ShaderOverviewViewer : OverviewViewer<ShaderOverviewData, ShaderInfo, ShaderOverviewModeManager>
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
                        new ColumnType("VariantStr", "Variant", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "")};
                case ShaderOverviewMode.Property:
                    return new ColumnType[] {
                        new ColumnType("Property", "Property", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
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
                default:
                    throw new NotImplementedException();
            }
        }

    }

}