using EditorCommon;
using System;
using UnityEditor;
using UnityEngine;

namespace AssetViewer
{
    public enum ModelViewerModer
    {
        ReadWrite = 0,
        TriangleCount,
        VertexCount,
        MeshData,
        OptimizeMesh,
        ImportMaterial,
        MeshCompress
    }

    public class ModelViewer : Viewer<ModelViewerData, ModelInfo, ModelViewerModeManager, ModelHealthInfoManager>
    {
        public ModelViewer(EditorWindow hostWindow) : base(hostWindow)
        {

        }
    }

    public class ModelViewerModeManager : ViewerModeManager
    {
        public override string[] GetMode()
        {
            return Enum.GetNames(typeof(ModelViewerModer));
        }

        public override ColumnType[] GetDataTable(string modelViewerMode)
        {
            ModelViewerModer modelViewerModeEnum = (ModelViewerModer)Enum.Parse(typeof(ModelViewerModer), modelViewerMode);
            switch (modelViewerModeEnum)
            {
                case ModelViewerModer.ReadWrite:
                    return new ColumnType[] {
                        new ColumnType("ReadWriteEnable", "R/W Enable", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case ModelViewerModer.ImportMaterial:
                    return new ColumnType[] {
                        new ColumnType("ImportMaterials", "ImportMaterials", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case ModelViewerModer.OptimizeMesh:
                    return new ColumnType[] {
                        new ColumnType("OptimizeMesh", "OptimizeMesh", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case ModelViewerModer.MeshData:
                    return new ColumnType[] {
                        new ColumnType("MeshDataStr", "MeshData", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case ModelViewerModer.MeshCompress:
                    return new ColumnType[] {
                        new ColumnType("MeshCompression", "MeshCompression", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case ModelViewerModer.VertexCount:
                    return new ColumnType[] {
                        new ColumnType("VertexRangeStr", "Vertex", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                case ModelViewerModer.TriangleCount:
                    return new ColumnType[] {
                        new ColumnType("TriangleRangeStr", "Triangle", ViewerConst.LeftWidth, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Count", "Count", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, ""),
                        new ColumnType("Memory", "Memory", (1.0f - ViewerConst.LeftWidth) / 2.0f, TextAnchor.MiddleCenter, "<fmt_bytes>")};
                default:
                    throw new NotImplementedException();
            }

        }

        public override ColumnType[] GetShowTable(string modelViewerMode)
        {
            ModelViewerModer modelViewerModeEnum = (ModelViewerModer)Enum.Parse(typeof(ModelViewerModer), modelViewerMode);
            switch (modelViewerModeEnum)
            {
                case ModelViewerModer.ReadWrite:
                case ModelViewerModer.ImportMaterial:
                case ModelViewerModer.OptimizeMesh:
                case ModelViewerModer.MeshData:
                case ModelViewerModer.MeshCompress:
                case ModelViewerModer.VertexCount:
                case ModelViewerModer.TriangleCount:
                    return new ColumnType[] {
                        new ColumnType("RealPath", "Path", 0.6f, TextAnchor.MiddleLeft, ""),
                        new ColumnType("MemSize", "Memory", 0.1f, TextAnchor.MiddleCenter, "<fmt_bytes>"),
                        new ColumnType("TotalMem", "TotalMemory", 0.1f, TextAnchor.MiddleCenter, "<fmt_bytes>"),
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