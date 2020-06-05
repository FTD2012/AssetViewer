using UnityEditor;
using System;

namespace ResourceFormat
{
    public enum ModelOverviewMode
    {
        ReadWrite = 0,
        ImportMaterial,
        OptimizeMesh,
        MeshData,
        MeshCompress,
        VertexCount,
        TriangleCount
    }

    public class ModelOverviewData : OverviewData
    {
        /// Don't modify variable name
        public int Count;
        public int Memory;
        public bool ReadWriteEnable;
        public bool ImportMaterials;
        public bool OptimizeMesh;
        public int MeshDataID;
        public string MeshDataStr;
        public ModelImporterMeshCompression MeshCompression = ModelImporterMeshCompression.Off;
        public string VertexRangeStr;
        public string TriangleRangeStr;

        private ModelOverviewMode _mode;

        public ModelOverviewData(string mode, ModelInfo modelInfo)
        {
            _mode = (ModelOverviewMode)Enum.Parse(typeof(ModelOverviewMode), mode);
            ReadWriteEnable = modelInfo.ReadWriteEnable;
            ImportMaterials = modelInfo.ImportMaterials;
            OptimizeMesh = modelInfo.OptimizeMesh;
            MeshDataID = modelInfo.GetMeshDataID();
            MeshDataStr = ModelInfo.GetMeshDataStr(MeshDataID);
            MeshCompression = modelInfo.MeshCompression;
            VertexRangeStr = modelInfo.GetVertexRangeStr();
            TriangleRangeStr = modelInfo.GetTriangleRangeStr();
        }

        public override bool IsMatch(BaseInfo modelInfo)
        {
            return isMatch((ModelInfo)modelInfo);
        }

        private bool isMatch(ModelInfo modelInfo)
        {
            switch (_mode)
            {
                case ModelOverviewMode.ReadWrite:
                    return ReadWriteEnable == modelInfo.ReadWriteEnable;
                case ModelOverviewMode.ImportMaterial:
                    return ImportMaterials == modelInfo.ImportMaterials;
                case ModelOverviewMode.OptimizeMesh:
                    return OptimizeMesh == modelInfo.OptimizeMesh;
                case ModelOverviewMode.MeshData:
                    return MeshDataID == modelInfo.GetMeshDataID();
                case ModelOverviewMode.MeshCompress:
                    return MeshCompression == modelInfo.MeshCompression;
                case ModelOverviewMode.VertexCount:
                    return VertexRangeStr == modelInfo.GetVertexRangeStr();
                case ModelOverviewMode.TriangleCount:
                    return TriangleRangeStr == modelInfo.GetTriangleRangeStr();
            }
            return false;
        }

        public override void AddObject(BaseInfo modelInfo)
        {
            addObject((ModelInfo)modelInfo);
        }

        private void addObject(ModelInfo modelInfo)
        {
            Memory += modelInfo.MemSize;
            _object.Add(modelInfo);
            Count++;
        }
    }
}