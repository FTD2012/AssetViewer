using System;
using UnityEngine;

namespace AssetViewer
{
    public class AudioOverviewData : OverviewData
    {
        /// Don't modify variable name
        public int Memory;
        public bool ForceToMono;
        public bool LoadInBackground;
        public bool Ambisonic;
        public AudioClipLoadType StandAloneAudioClipLoadType;
        public AudioClipLoadType AndroidAudioClipLoadType;
        public AudioClipLoadType iOSAudioClipLoadType;
        public AudioCompressionFormat StandAloneAudioCompressionFormat;
        public AudioCompressionFormat AndroidAudioCompressionFormat;
        public AudioCompressionFormat iOSAudioCompressionFormat;

        private AudioOverviewMode _mode;
        public AudioOverviewData(string mode, AudioInfo audioInfo)
        {
            _mode = (AudioOverviewMode)Enum.Parse(typeof(AudioOverviewMode), mode);
            ForceToMono = audioInfo.ForceToMono;
            LoadInBackground = audioInfo.LoadInBackground;
            Ambisonic = audioInfo.Ambisonic;
            StandAloneAudioClipLoadType = audioInfo.StandAloneAudioClipLoadType;
            AndroidAudioClipLoadType = audioInfo.AndroidAudioClipLoadType;
            iOSAudioClipLoadType = audioInfo.iOSAudioClipLoadType;
            StandAloneAudioCompressionFormat = audioInfo.StandAloneAudioCompressionFormat;
            AndroidAudioCompressionFormat = audioInfo.AndroidAudioCompressionFormat;
            iOSAudioCompressionFormat = audioInfo.iOSAudioCompressionFormat;
        }

        public override bool IsMatch(BaseInfo audioInfo)
        {
            return isMatch((AudioInfo)audioInfo);
        }

        private bool isMatch(AudioInfo audioInfo)
        {
            switch (_mode)
            {
                case AudioOverviewMode.MONO:
                    return ForceToMono == audioInfo.ForceToMono;
                case AudioOverviewMode.LoadInBackground:
                    return LoadInBackground == audioInfo.LoadInBackground;
                case AudioOverviewMode.Ambisonic:
                    return Ambisonic == audioInfo.Ambisonic;
                case AudioOverviewMode.AndroidLoadType:
                    return AndroidAudioClipLoadType == audioInfo.AndroidAudioClipLoadType;
                case AudioOverviewMode.iOSLoadType:
                    return iOSAudioClipLoadType == audioInfo.iOSAudioClipLoadType;
                case AudioOverviewMode.AndroidCompressionFormat:
                    return AndroidAudioCompressionFormat == audioInfo.AndroidAudioCompressionFormat;
                case AudioOverviewMode.iOSCompressionFormat:
                    return iOSAudioCompressionFormat == audioInfo.iOSAudioCompressionFormat;
            }
            return false;
        }

        public override int GetMatchHealthCount(object obj)
        {
            int count = 0;

            foreach (AudioInfo audioInfo in _object)
            {
                switch (_mode)
                {
                    case AudioOverviewMode.MONO:
                        count += audioInfo.ForceToMono == (bool)obj ? 1 : 0;
                        break;
                    case AudioOverviewMode.LoadInBackground:
                        count += audioInfo.LoadInBackground == (bool)obj ? 1 : 0;
                        break;
                    case AudioOverviewMode.Ambisonic:
                        count += audioInfo.Ambisonic == (bool)obj ? 1 : 0;
                        break;
                    case AudioOverviewMode.AndroidLoadType:
                        count += audioInfo.AndroidAudioClipLoadType == (AudioClipLoadType)obj ? 1 : 0;
                        break;
                    case AudioOverviewMode.iOSLoadType:
                        count += audioInfo.iOSAudioClipLoadType == (AudioClipLoadType)obj ? 1 : 0;
                        break;
                    case AudioOverviewMode.AndroidCompressionFormat:
                        count += audioInfo.AndroidAudioCompressionFormat == (AudioCompressionFormat)obj ? 1 : 0;
                        break;
                    case AudioOverviewMode.iOSCompressionFormat:
                        count += audioInfo.iOSAudioCompressionFormat == (AudioCompressionFormat)obj ? 1 : 0;
                        break;
                }
            }
            return count;
        }

        public override void AddObject(BaseInfo audioInfo)
        {
            addObject((AudioInfo)audioInfo);
        }

        private void addObject(AudioInfo audioInfo)
        {
            Memory += audioInfo.MemSize;
            _object.Add(audioInfo);
            Count++;
        }
    }
}