using EditorCommon;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ResourceFormat
{
    public class ParticleInfo : BaseInfo
    {
        public string RealPath;
        public int MaxParticles;
        public float Duration;
        public bool PlayOnAwake;
        public bool Looping;

        private static Dictionary<string, ParticleInfo> _dictParticleInfo = new Dictionary<string, ParticleInfo>();

        public static ParticleInfo[] CreateParticleInfo(string assetPath)
        {
            if (!EditorPath.IsPrefab(assetPath))
            {
                return null;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            ParticleSystem[] particleSystem = prefab.GetComponentsInChildren<ParticleSystem>();
            ParticleInfo[] particleInfo = new ParticleInfo[particleSystem.Length];

            for (int i  = 0; i < particleSystem.Length; i++)
            {
                particleInfo[i] = new ParticleInfo();
                particleInfo[i].Path = assetPath;
                particleInfo[i].RealPath = assetPath + "/" + OverviewTableConst.GetPath(particleSystem[i].transform);
                particleInfo[i].MaxParticles = particleSystem[i].main.maxParticles;
                particleInfo[i].Duration = particleSystem[i].main.duration;
                particleInfo[i].PlayOnAwake = particleSystem[i].main.playOnAwake;
                particleInfo[i].Looping = particleSystem[i].main.loop;
            }


            return particleInfo;
        }

        public static List<ParticleInfo> GetInfoByDirectory(string dir)
        {
            List<ParticleInfo> particleInfoList = new List<ParticleInfo>();
            List<string> list = new List<string>();
            EditorPath.ScanDirectoryFile(dir, true, list);
            for (int i = 0; i < list.Count; ++i)
            {
                string assetPath = EditorPath.FormatAssetPath(list[i]);
                string name = System.IO.Path.GetFileName(assetPath);
                EditorUtility.DisplayProgressBar("获取贴图数据", name, (i * 1.0f) / list.Count);
                ParticleInfo[] particleInfo = CreateParticleInfo(assetPath);
                if (particleInfo != null)
                {
                    particleInfoList.AddRange(particleInfo);
                }
            }
            EditorUtility.ClearProgressBar();
            return particleInfoList;
        }



    }

}