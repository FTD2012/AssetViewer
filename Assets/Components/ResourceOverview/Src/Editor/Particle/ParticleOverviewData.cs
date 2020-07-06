using UnityEditor;
using System;

namespace ResourceFormat
{    
    public class ParticleOverviewData : OverviewData
    {
        /// Don't modify variable name
        public int Count;
        public int MaxParticle;
        public int SizeIndex;
        public string SizeStr;
        public float Duration;
        public int DurationIndex;
        public string DurtationStr;
        public bool PlayOnAwake;
        public bool Looping;

        private ParticleOverviewMode _mode;

        public ParticleOverviewData(string mode, ParticleInfo particleInfo)
        {
            _mode = (ParticleOverviewMode)Enum.Parse(typeof(ParticleOverviewMode), mode);
            MaxParticle = particleInfo.MaxParticles;
            SizeIndex = OverviewTableConst.GetParticleSizeIndex(particleInfo.MaxParticles);
            SizeStr = OverviewTableConst.ParticleSizeStr[SizeIndex];
            Duration = particleInfo.Duration;
            DurationIndex = OverviewTableConst.GetDurationIndex(particleInfo.Duration);
            DurtationStr = OverviewTableConst.DurationSizeStr[DurationIndex];
            PlayOnAwake = particleInfo.PlayOnAwake;
            Looping = particleInfo.Looping;
        }

        public override bool IsMatch(BaseInfo modelInfo)
        {
            return isMatch((ParticleInfo)modelInfo);
        }


        private bool isMatch(ParticleInfo particleInfo)
        {
            switch (_mode)
            {
                case ParticleOverviewMode.MaxParticle:
                    return SizeIndex == OverviewTableConst.GetParticleSizeIndex(particleInfo.MaxParticles);
                case ParticleOverviewMode.Duration:
                    return DurationIndex == OverviewTableConst.GetDurationIndex(particleInfo.Duration);
                case ParticleOverviewMode.PlayOnAwake:
                    return PlayOnAwake == particleInfo.PlayOnAwake;
                case ParticleOverviewMode.Looping:
                    return Looping == particleInfo.Looping;
            }
            return false;
        }

        public override int GetMatchHealthCount(object obj)
        {
            int count = 0;

            foreach (ParticleInfo particleInfo in _object)
            {
                switch (_mode)
                {
                    case ParticleOverviewMode.MaxParticle:
                        count += particleInfo.MaxParticles >= (int)obj ? 1 : 0;
                        break;
                }
            }

            return count;
        }

        public override void AddObject(BaseInfo modelInfo)
        {
            addObject((ParticleInfo)modelInfo);
        }

        private void addObject(ParticleInfo modelInfo)
        {
            _object.Add(modelInfo);
            Count++;
        }

    }
}