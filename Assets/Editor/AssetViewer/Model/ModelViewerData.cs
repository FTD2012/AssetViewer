using UnityEditor;
using System;

namespace AssetViewer
{
    public enum ModelViewerMode
    {
        ReadWrite = 0,
        ImportMaterial,
        OptimizeMesh,
        MeshData,
        MeshCompress,
        VertexCount,
        TriangleCount
    }

    public class ModelViewerData : ViewerData
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

        private ModelViewerMode _mode;

        public ModelViewerData(string mode, ModelInfo modelInfo)
        {
            _mode = (ModelViewerMode)Enum.Parse(typeof(ModelViewerMode), mode);
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
                case ModelViewerMode.ReadWrite:
                    return ReadWriteEnable == modelInfo.ReadWriteEnable;
                case ModelViewerMode.ImportMaterial:
                    return ImportMaterials == modelInfo.ImportMaterials;
                case ModelViewerMode.OptimizeMesh:
                    return OptimizeMesh == modelInfo.OptimizeMesh;
                case ModelViewerMode.MeshData:
                    return MeshDataID == modelInfo.GetMeshDataID();
                case ModelViewerMode.MeshCompress:
                    return MeshCompression == modelInfo.MeshCompression;
                case ModelViewerMode.VertexCount:
                    return VertexRangeStr == modelInfo.GetVertexRangeStr();
                case ModelViewerMode.TriangleCount:
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
                    case ModelViewerMode.ReadWrite:
                        count += modelInfo.ReadWriteEnable == (bool)obj ? 1 : 0;
                        break;
                    case ModelViewerMode.TriangleCount:
                        count += modelInfo.triangleCount >= (int)obj ? 1 : 0;
                        break;
                    case ModelViewerMode.VertexCount:
                        count += modelInfo.vertexCount >= (int)obj ? 1 : 0;
                        break;
                    case ModelViewerMode.OptimizeMesh:
                        count += modelInfo.OptimizeMesh == (bool)obj ? 1 : 0;
                        break;
                    case ModelViewerMode.MeshData:
                        count += modelInfo.GetMeshDataID() == (int)obj ? 1 : 0;
                        break;
                    case ModelViewerMode.ImportMaterial:
                        count += modelInfo.ImportMaterials == (bool)obj ? 1 : 0;
                        break;
                    case ModelViewerMode.MeshCompress:
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