using UnityEditor;
using System;

namespace AssetViewer
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

        public override int GetMatchHealthCount(object obj)
        {
            int count = 0;

            foreach (ModelInfo modelInfo in _object)
            {
                switch (_mode)
                {
                    case ModelOverviewMode.ReadWrite:
                        count += modelInfo.ReadWriteEnable == (bool)obj ? 1 : 0;
                        break;
                    case ModelOverviewMode.TriangleCount:
                        count += modelInfo.triangleCount >= (int)obj ? 1 : 0;
                        break;
                    case ModelOverviewMode.VertexCount:
                        count += modelInfo.vertexCount >= (int)obj ? 1 : 0;
                        break;
                    case ModelOverviewMode.OptimizeMesh:
                        count += modelInfo.OptimizeMesh == (bool)obj ? 1 : 0;
                        break;
                    case ModelOverviewMode.MeshData:
                        count += modelInfo.GetMeshDataID() == (int)obj ? 1 : 0;
                        break;
                    case ModelOverviewMode.ImportMaterial:
                        count += modelInfo.ImportMaterials == (bool)obj ? 1 : 0;
                        break;
                    case ModelOverviewMode.MeshCompress:
                        count += modelInfo.MeshCompression == (ModelImporterMeshCompression)obj ? 1 : 0;
                        break;
                }
            }
            return count;
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