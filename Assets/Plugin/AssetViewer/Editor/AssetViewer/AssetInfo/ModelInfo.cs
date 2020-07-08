using EditorCommon;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text;

namespace ResourceFormat
{
    public class ModelInfo : BaseInfo
    {
        public bool ReadWriteEnable = false;
        public bool OptimizeMesh = false;
        public bool ImportMaterials;
        public bool ImportAnimation;
        public ModelImporterMeshCompression MeshCompression = ModelImporterMeshCompression.Off;
        public bool bHasUV;
        public bool bHasUV2;
        public bool bHasUV3;
        public bool bHasUV4;
        public bool bHasColor;
        public bool bHasNormal;
        public bool bHasTangent;
        public int vertexCount;
        public int triangleCount;
        public string RealPath;
        public int TotalMem;

        private static int _loadCount = 0;

        public int GetMeshDataID()
        {
            int meshDataIndex = _WorkData(0, bHasUV);
            meshDataIndex = _WorkData(meshDataIndex, bHasUV2);
            meshDataIndex = _WorkData(meshDataIndex, bHasUV3);
            meshDataIndex = _WorkData(meshDataIndex, bHasUV4);
            meshDataIndex = _WorkData(meshDataIndex, bHasColor);
            meshDataIndex = _WorkData(meshDataIndex, bHasNormal);
            meshDataIndex = _WorkData(meshDataIndex, bHasTangent);

            return meshDataIndex;
        }

        public int GetVertexRangeID()
        {
            return vertexCount / OverviewTableConst.VertexCountMod;
        }

        public string GetVertexRangeStr()
        {
            int index = GetVertexRangeID();
            return string.Format("{0}-{1}",
                index * OverviewTableConst.VertexCountMod,
                (index + 1) * OverviewTableConst.VertexCountMod - 1);
        }

        public int GetTriangleRangeID()
        {
            return triangleCount / OverviewTableConst.TriangleCountMod;
        }

        public string GetTriangleRangeStr()
        {
            int index = GetTriangleRangeID();
            return string.Format("{0}-{1}",
                index * OverviewTableConst.TriangleCountMod,
                (index + 1) * OverviewTableConst.TriangleCountMod - 1);
        }

        public static string GetMeshDataStr(int key)
        {
            bool[] bData = new bool[7];
            for (int i = 0; i < 7; ++i)
            {
                bData[i] = ((key >> i) & 1) > 0;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("vertices");
            for (int i = 6; i >= 0; --i)
            {
                if (bData[i])
                {
                    sb.Append("," + OverviewTableConst.MeshDataStr[i]);
                }
            }

            return sb.ToString();
        }

        private static int _WorkData(int data, bool flag)
        {
            if (flag)
                return (data << 1) | 1;
            else
                return data << 1;
        }

        public static List<ModelInfo> CreateModelInfo(string assetPath)
        {
            if (!EditorPath.IsModel(assetPath))
            {
                return null;
            }

            ModelImporter modelImport = AssetImporter.GetAtPath(assetPath) as ModelImporter;
            Object[] meshArray = AssetDatabase.LoadAllAssetsAtPath(assetPath);
            List<ModelInfo> modelInfoList = new List<ModelInfo>();
            int TotalModelMem = EditorTool.CalculateModelSizeBytes(assetPath);

            if (modelImport == null || meshArray.Length <= 0)
            {
                return null;
            }

            foreach (Object meshObject in meshArray)
            {
                Mesh mesh = meshObject as Mesh;
                if (mesh != null)
                {
                    ModelInfo modelInfo = new ModelInfo();
                    modelInfo.Path = assetPath;
                    modelInfo.RealPath = assetPath + "/" + mesh.name;
                    modelInfo.ReadWriteEnable = modelImport.isReadable;
                    modelInfo.OptimizeMesh = modelImport.optimizeMesh;
                    modelInfo.ImportMaterials = modelImport.importMaterials;
                    modelInfo.ImportAnimation = modelImport.importAnimation;
                    modelInfo.MeshCompression = modelImport.meshCompression;
                    
                    modelInfo.bHasUV = mesh.uv != null && mesh.uv.Length != 0;
                    modelInfo.bHasUV2 = mesh.uv2 != null && mesh.uv2.Length != 0;
                    modelInfo.bHasUV3 = mesh.uv3 != null && mesh.uv3.Length != 0;
                    modelInfo.bHasUV4 = mesh.uv4 != null && mesh.uv4.Length != 0;
                    modelInfo.bHasColor = mesh.colors != null && mesh.colors.Length != 0;
                    modelInfo.bHasNormal = mesh.normals != null && mesh.normals.Length != 0;
                    modelInfo.bHasTangent = mesh.tangents != null && mesh.tangents.Length != 0;
                    modelInfo.vertexCount = mesh.vertexCount;
                    modelInfo.triangleCount = mesh.triangles.Length / 3;

                    modelInfo.MemSize = EditorTool.GetRuntimeMemorySize(mesh);
                    modelInfo.TotalMem = TotalModelMem;

                    modelInfoList.Add(modelInfo);

                    if (++_loadCount % 256 == 0)
                    {
                        Resources.UnloadUnusedAssets();
                    }
                }
                else
                {
                    if ((!(meshObject is GameObject)) && (!(meshObject is Component)))
                    {
                        Resources.UnloadAsset(meshObject);
                    }
                }
            }

            return modelInfoList;
        }

        public static List<ModelInfo> GetInfoByDirectory(string dir)
        {
            List<ModelInfo> modelInfoList = new List<ModelInfo>();
            List<string> list = new List<string>();
            EditorPath.ScanDirectoryFile(dir, true, list);
            for (int i = 0; i < list.Count; ++i)
            {
                string assetPath = EditorPath.FormatAssetPath(list[i]);
                List<ModelInfo> dummyModelInfoList = CreateModelInfo(assetPath);
                if (dummyModelInfoList != null)
                {
                    modelInfoList.AddRange(dummyModelInfoList);
                }
            }

            return modelInfoList;
        }

    }
}
