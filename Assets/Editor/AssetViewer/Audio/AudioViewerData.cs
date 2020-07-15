using System;
using UnityEngine;

namespace AssetViewer
{
    public class AudioViewerData : ViewerData
    {
        /// Don't modify variable name
        public int Memory;
        public int TotalOriginSize;
        public int TotalCompressedSize;
        public bool ForceToMono;
        public bool LoadInBackground;
        public bool Ambisonic;
        public AudioClipLoadType StandAloneAudioClipLoadType;
        public AudioClipLoadType AndroidAudioClipLoadType;
        public AudioClipLoadType iOSAudioClipLoadType;
        public AudioCompressionFormat StandAloneAudioCompressionFormat;
        public AudioCompressionFormat AndroidAudioCompressionFormat;
        public AudioCompressionFormat iOSAudioCompressionFormat;
        public int OriginSize;
        public int CompressedSize;
        public string All;

        private AudioViewerMode _mode;

        public AudioViewerData(string mode, AudioInfo audioInfo)
        {
            _mode = (AudioViewerMode)Enum.Parse(typeof(AudioViewerMode), mode);
            ForceToMono = audioInfo.ForceToMono;
            LoadInBackground = audioInfo.LoadInBackground;
            Ambisonic = audioInfo.Ambisonic;
            StandAloneAudioClipLoadType = audioInfo.StandAloneAudioClipLoadType;
            AndroidAudioClipLoadType = audioInfo.AndroidAudioClipLoadType;
            iOSAudioClipLoadType = audioInfo.iOSAudioClipLoadType;
            StandAloneAudioCompressionFormat = audioInfo.StandAloneAudioCompressionFormat;
            AndroidAudioCompressionFormat = audioInfo.AndroidAudioCompressionFormat;
            iOSAudioCompressionFormat = audioInfo.iOSAudioCompressionFormat;
            OriginSize = audioInfo.OriginSize;
            CompressedSize = audioInfo.CompressedSize;
            All = "All";
        }

        public override bool IsMatch(BaseInfo audioInfo)
        {
            return isMatch((AudioInfo)audioInfo);
        }

        private bool isMatch(AudioInfo audioInfo)
        {
            switch (_mode)
            {
                case AudioViewerMode.Size:
                    return true;
                case AudioViewerMode.MONO:
                    return ForceToMono == audioInfo.ForceToMono;
                case AudioViewerMode.LoadInBackground:
                    return LoadInBackground == audioInfo.LoadInBackground;
                case AudioViewerMode.Ambisonic:
                    return Ambisonic == audioInfo.Ambisonic;
                case AudioViewerMode.AndroidLoadType:
                    return AndroidAudioClipLoadType == audioInfo.AndroidAudioClipLoadType;
                case AudioViewerMode.iOSLoadType:
                    return iOSAudioClipLoadType == audioInfo.iOSAudioClipLoadType;
                case AudioViewerMode.AndroidCompressionFormat:
                    return AndroidAudioCompressionFormat == audioInfo.AndroidAudioCompressionFormat;
                case AudioViewerMode.iOSCompressionFormat:
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
                    case AudioViewerMode.Size:
                        count += audioInfo.CompressedSize > (int)obj ? 1 : 0;
                        break;
                    case AudioViewerMode.MONO:
                        count += audioInfo.ForceToMono == (bool)obj ? 1 : 0;
                        break;
                    case AudioViewerMode.LoadInBackground:
                        count += audioInfo.LoadInBackground == (bool)obj ? 1 : 0;
                        break;
                    case AudioViewerMode.Ambisonic:
                        count += audioInfo.Ambisonic == (bool)obj ? 1 : 0;
                        break;
                    case AudioViewerMode.AndroidLoadType:
                        count += audioInfo.AndroidAudioClipLoadType == (AudioClipLoadType)obj ? 1 : 0;
                        break;
                    case AudioViewerMode.iOSLoadType:
                        count += audioInfo.iOSAudioClipLoadType == (AudioClipLoadType)obj ? 1 : 0;
                        break;
                    case AudioViewerMode.AndroidCompressionFormat:
                        count += audioInfo.AndroidAudioCompressionFormat == (AudioCompressionFormat)obj ? 1 : 0;
                        break;
                    case AudioViewerMode.iOSCompressionFormat:
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
            TotalOriginSize += audioInfo.OriginSize;
            TotalCompressedSize += audioInfo.CompressedSize;
            Memory += audioInfo.MemSize;
            _object.Add(audioInfo);
            Count++;
        }
    }
}