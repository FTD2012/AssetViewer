﻿using EditorCommon;
using System.Collections.Generic;
using UnityEditor;

namespace ResourceFormat
{
    public class ModelImportData : ImportData
    {
        public bool ReadWriteEnable = false;
        public bool OptimizeMesh = false;
        public bool ImportMaterials = false;
        public bool ImportAnimation = false;
        public bool ImportUV2 = true;
        public bool ImportUV3 = true;
        public bool ImportUV4 = true;
        public bool ImportNormal = true;
        public bool ImportTangent = true;
        public ModelImporterMeshCompression MeshCompression = ModelImporterMeshCompression.Off;

        public override bool IsMatch(string path)
        {
            return EditorPath.IsModel(path) && base.IsMatch(path);
        }
        public override void CopyData(ImportData data)
        {
            ModelImportData tData = data as ModelImportData;
            if (tData == null)
                return;

            base.CopyData(data);
            ReadWriteEnable = tData.ReadWriteEnable;
            OptimizeMesh = tData.OptimizeMesh;
            ImportMaterials = tData.ImportMaterials;
            ImportAnimation = tData.ImportAnimation;
            ImportUV2 = tData.ImportUV2;
            ImportUV3 = tData.ImportUV3;
            ImportUV4 = tData.ImportUV4;
            ImportNormal = tData.ImportNormal;
            ImportTangent = tData.ImportTangent;
            MeshCompression = tData.MeshCompression;
        }
        public bool IsFormatModel(ModelInfo tInfo)
        {
            if (tInfo.OptimizeMesh != OptimizeMesh)
                return false;
            if (tInfo.ReadWriteEnable != ReadWriteEnable)
                return false;
            if (tInfo.ImportAnimation != ImportAnimation)
                return false;
            if (tInfo.ImportMaterials != ImportMaterials)
                return false;
            if (tInfo.MeshCompression != MeshCompression)
                return false;
            return true;
        }

        public override void ClearObject()
        {
            m_initUnFormatList = false;
            base.ClearObject();
        }
        public void AddObject(ModelInfo textureInfo)
        {
            TotalCount = TotalCount + 1;
            TotalMemuse = TotalMemuse + textureInfo.MemSize;
            _object.Add(textureInfo);
            if (m_initUnFormatList && IsFormatModel(textureInfo))
            {
                _unFortmatObject.Add(textureInfo);
            }
        }
        public List<object> GetObjects(bool unformat)
        {
            if (!unformat)
            {
                return _object;
            }
            else
            {
                if (!m_initUnFormatList)
                {
                    m_initUnFormatList = true;
                    _InitUnFormatList();
                }
                return _unFortmatObject;
            }
        }

        private void _InitUnFormatList()
        {
            for (int i = 0; i < _object.Count; ++i)
            {
                ModelInfo modelInfo = _object[i] as ModelInfo;
                if (modelInfo == null)
                    continue;
                string name = System.IO.Path.GetFileName(modelInfo.Path);
                EditorUtility.DisplayProgressBar("更新非法模型数据", name, (i * 1.0f) / _object.Count);
                if (!IsFormatModel(modelInfo))
                {
                    _unFortmatObject.Add(modelInfo);
                }
            }
            EditorUtility.ClearProgressBar();
        }

        private bool m_initUnFormatList = false;
    }
}
