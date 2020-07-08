using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using EditorCommon;

namespace AssetViewer
{
    public class AnimationInfo : BaseInfo
    {
        public ModelImporterAnimationType AnimationType = ModelImporterAnimationType.None;
        public ModelImporterAnimationCompression AnimationCompression = ModelImporterAnimationCompression.Off;

        private static int _loadCount = 0;
        private static Dictionary<string, AnimationInfo> _dictMatInfo = new Dictionary<string, AnimationInfo>();

        public static AnimationInfo CreateAnimationInfo(string assetPath)
        {
            AnimationInfo mInfo = null;
            if (!_dictMatInfo.TryGetValue(assetPath, out mInfo))
            {
                mInfo = new AnimationInfo();
                _dictMatInfo.Add(assetPath, mInfo);
            }

            ModelImporter tImporter = AssetImporter.GetAtPath(assetPath) as ModelImporter;
            if (tImporter == null || tImporter.clipAnimations == null)
                return null;

            mInfo.Path = assetPath;
            mInfo.AnimationType = tImporter.animationType;
            mInfo.AnimationCompression = tImporter.animationCompression;
            mInfo.MemSize = EditorTool.CalculateAnimationSizeBytes(assetPath);

            if (++_loadCount % 256 == 0)
            {
                Resources.UnloadUnusedAssets();
            }

            return mInfo;
        }
    }
}
