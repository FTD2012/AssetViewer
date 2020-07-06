using EditorCommon;
using System;
using UnityEditor;
using UnityEngine;

namespace ResourceFormat
{
    public enum ModelOverviewModer
    {
        ReadWrite = 0,
        ImportMaterial,
        OptimizeMesh,
        MeshData,
        MeshCompress,
        VertexCount,
        TriangleCount
    }

    public class ModelOverviewViewer : OverviewViewer<ModelOverviewData, ModelInfo, ModelOverviewModeManager, ModelHealthInfoManager>
    {
        public ModelOverviewViewer(EditorWindow hostWindow) : base(hostWindow)
        {

        }
    }

    public class ModelOverviewModeManager : OverviewModeManager
    {
        public override string[] GetMode()
        {
            return Enum.GetNames(typeof(ModelOverviewModer));
        }

        public override ColumnType[] GetDataTable(string modelOverviewMode)
        {
            ModelOverviewModer modelOverviewModeEnum = (ModelOverviewModer)Enum.Parse(typeof(ModelOverviewModer), modelOverviewMode);
            switch (modelOverviewModeEnum)
            {
                case ModelOverviewModer.ReadWrite:
                    return new ColumnType[] {
                        new ColumnType("ReadWriteEnable", "R/W Enable", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case ModelOverviewModer.ImportMaterial:
                    return new ColumnType[] {
                        new ColumnType("ImportMaterials", "ImportMaterials", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case ModelOverviewModer.OptimizeMesh:
                    return new ColumnType[] {
                        new ColumnType("OptimizeMesh", "OptimizeMesh", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case ModelOverviewModer.MeshData:
                    return new ColumnType[] {
                        new ColumnType("MeshDataStr", "MeshData", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case ModelOverviewModer.MeshCompress:
                    return new ColumnType[] {
                        new ColumnType("MeshCompression", "MeshCompression", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case ModelOverviewModer.VertexCount:
                    return new ColumnType[] {
                        new ColumnType("VertexRangeStr", "Vertex", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case ModelOverviewModer.TriangleCount:
                    return new ColumnType[] {
                        new ColumnType("TriangleRangeStr", "Triangle", OverviewTableConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - OverviewTableConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                default:
                    throw new NotImplementedException();
            }

        }

        public override ColumnType[] GetShowTable(string modelOverviewMode)
        {
            ModelOverviewModer modelOverviewModeEnum = (ModelOverviewModer)Enum.Parse(typeof(ModelOverviewModer), modelOverviewMode);
            switch (modelOverviewModeEnum)
            {
                case ModelOverviewModer.ReadWrite:
                case ModelOverviewModer.ImportMaterial:
                case ModelOverviewModer.OptimizeMesh:
                case ModelOverviewModer.MeshData:
                case ModelOverviewModer.MeshCompress:
                case ModelOverviewModer.VertexCount:
                case ModelOverviewModer.TriangleCount:
                    return new ColumnType[] {
                        new ColumnType("Path", "Path", 0.7f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("MemSize", "Memory", 0.1f, TextAnchor.MiddleCenter, "<fmt_bytes>"),
                        new ColumnType("vertexCount", "VertexCount", 0.1f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("triangleCount", "TriangleCount", 0.1f, TextAnchor.MiddleCenter, "")};
                default:
                    throw new NotImplementedException();
            }


        }


    }

    public class ModelHealthInfoManager : HealthInfoManager
    {

    }

}