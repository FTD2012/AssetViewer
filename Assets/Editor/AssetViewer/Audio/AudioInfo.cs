using EditorCommon;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace AssetViewer
{
    public class AudioInfo : BaseInfo
    {
        public bool ForceToMono;
        public bool LoadInBackground;
        public bool Ambisonic;
        public int ImportedSize;
        public AudioClipLoadType StandAloneAudioClipLoadType;
        public AudioClipLoadType AndroidAudioClipLoadType;
        public AudioClipLoadType iOSAudioClipLoadType;
        public AudioCompressionFormat StandAloneAudioCompressionFormat;
        public AudioCompressionFormat AndroidAudioCompressionFormat;
        public AudioCompressionFormat iOSAudioCompressionFormat;
        public int OriginSize;
        public int CompressedSize;

        private static int _loadCount = 0;
        private static Dictionary<string, AudioInfo> _dictAudioInfo = new Dictionary<string, AudioInfo>();

        public static AudioInfo CreateAudioInfo(string assetPath)
        {
            if (!EditorPath.IsAudio(assetPath))
            {
                return null;
            }

            AudioInfo audioInfo = null;
            if (!_dictAudioInfo.TryGetValue(assetPath, out audioInfo))
            {
                audioInfo = new AudioInfo();
                _dictAudioInfo.Add(assetPath, audioInfo);
            }

            AudioImporter audioImporter = AssetImporter.GetAtPath(assetPath) as AudioImporter;
            AudioClip audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(assetPath);
            if (audioImporter == null || audioClip == null)
            {
                Debug.LogError("audioImporter is empty");
                return null;
            }

            audioInfo.Path = assetPath;
            audioInfo.ForceToMono = audioImporter.forceToMono;
            audioInfo.LoadInBackground = audioImporter.loadInBackground;
            audioInfo.Ambisonic = audioImporter.ambisonic;
            audioInfo.MemSize = EditorTool.GetRuntimeMemorySize(audioClip);
            audioInfo.ImportedSize = EditorTool.GetRuntimeMemorySize(audioClip);
            audioInfo.StandAloneAudioClipLoadType = audioImporter.GetOverrideSampleSettings("Standalone").loadType;
            audioInfo.AndroidAudioClipLoadType = audioImporter.GetOverrideSampleSettings("Android").loadType;
            audioInfo.iOSAudioClipLoadType = audioImporter.GetOverrideSampleSettings("iOS").loadType;
            audioInfo.StandAloneAudioCompressionFormat = audioImporter.GetOverrideSampleSettings("Standalone").compressionFormat;
            audioInfo.AndroidAudioCompressionFormat = audioImporter.GetOverrideSampleSettings("Android").compressionFormat;
            audioInfo.iOSAudioCompressionFormat = audioImporter.GetOverrideSampleSettings("iOS").compressionFormat;
            audioInfo.OriginSize = ViewerConst.GetInternalValue<int>(audioImporter, "get_origSize", null);
            audioInfo.CompressedSize = ViewerConst.GetInternalValue<int>(audioImporter, "get_compSize", null);

            if (++_loadCount % 256 == 0)
            {
                Resources.UnloadUnusedAssets();
            }

            return audioInfo;
        }

        public static List<AudioInfo> GetInfoByDirectory(string dir)
        {
            List<AudioInfo> audioInfoList = new List<AudioInfo>();
            List<string> list = new List<string>();
            EditorPath.ScanDirectoryFile(dir, true, list);
            for (int i = 0; i < list.Count; ++i)
            {
                string assetPath = EditorPath.FormatAssetPath(list[i]);
                string name = System.IO.Path.GetFileName(assetPath);
                EditorUtility.DisplayProgressBar("获取音频数据", name, (i * 1.0f) / list.Count);
                AudioInfo audioInfo = CreateAudioInfo(assetPath);
                if (audioInfo != null)
                {
                    audioInfoList.Add(audioInfo);
                }
            }
            EditorUtility.ClearProgressBar();
            return audioInfoList;
        }
    }
}
