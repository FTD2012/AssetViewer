using EditorCommon;
using System.Collections.Generic;
using UnityEditor;

namespace ResourceFormat
{
    public class AnimationImportData : ImportData
    {
        public ModelImporterAnimationType AnimationType = ModelImporterAnimationType.None;
        public ModelImporterAnimationCompression AnimationCompression = ModelImporterAnimationCompression.Off;

        public override bool IsMatch(string path)
        {
            return EditorPath.IsAnimation(path) && base.IsMatch(path);
        }
        public override void CopyData(ImportData data)
        {
            AnimationImportData tData = data as AnimationImportData;
            if (tData == null)
                return;

            base.CopyData(data);
            AnimationType = tData.AnimationType;
            AnimationCompression = tData.AnimationCompression;
        }
        public override void ClearObject()
        {
            m_initUnFormatList = false;
            base.ClearObject();
        }
        public void AddObject(AnimationInfo textureInfo)
        {
            TotalCount = TotalCount + 1;
            TotalMemuse = TotalMemuse + textureInfo.MemSize;
            _object.Add(textureInfo);
            if (m_initUnFormatList && !IsFormatAnimation(textureInfo))
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
        public bool IsFormatAnimation(AnimationInfo tInfo)
        {
            if (tInfo.AnimationType != AnimationType)
                return false;
            if (tInfo.AnimationCompression != AnimationCompression)
                return false;
            return true;
        }
        private void _InitUnFormatList()
        {
            for (int i = 0; i < _object.Count; ++i)
            {
                AnimationInfo aniInfo = _object[i] as AnimationInfo;
                if (aniInfo == null)
                    continue;
                string name = System.IO.Path.GetFileName(aniInfo.Path);
                EditorUtility.DisplayProgressBar("更新非法动作数据", name, (i * 1.0f) / _object.Count);
                if (!IsFormatAnimation(aniInfo))
                {
                    _unFortmatObject.Add(aniInfo);
                }
            }
            EditorUtility.ClearProgressBar();
        }

        private bool m_initUnFormatList = false;
    }
}
