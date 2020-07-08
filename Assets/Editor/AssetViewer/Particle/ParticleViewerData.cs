using System;

namespace AssetViewer
{
    public class ParticleViewerData : ViewerData
    {
        /// Don't modify variable name
        public int MaxParticle;
        public int SizeIndex;
        public string SizeStr;
        public float Duration;
        public int DurationIndex;
        public string DurtationStr;
        public bool PlayOnAwake;
        public bool Looping;

        private ParticleViewerMode _mode;

        public ParticleViewerData(string mode, ParticleInfo particleInfo)
        {
            _mode = (ParticleViewerMode)Enum.Parse(typeof(ParticleViewerMode), mode);
            MaxParticle = particleInfo.MaxParticles;
            SizeIndex = ViewerConst.GetParticleSizeIndex(particleInfo.MaxParticles);
            SizeStr = ViewerConst.ParticleSizeStr[SizeIndex];
            Duration = particleInfo.Duration;
            DurationIndex = ViewerConst.GetDurationIndex(particleInfo.Duration);
            DurtationStr = ViewerConst.DurationSizeStr[DurationIndex];
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
                case ParticleViewerMode.MaxParticle:
                    return SizeIndex == ViewerConst.GetParticleSizeIndex(particleInfo.MaxParticles);
                case ParticleViewerMode.Duration:
                    return DurationIndex == ViewerConst.GetDurationIndex(particleInfo.Duration);
                case ParticleViewerMode.PlayOnAwake:
                    return PlayOnAwake == particleInfo.PlayOnAwake;
                case ParticleViewerMode.Looping:
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
                    case ParticleViewerMode.MaxParticle:
                        count += particleInfo.MaxParticles > (int)obj ? 1 : 0;
                        break;
                    case ParticleViewerMode.Duration:
                        count += particleInfo.Duration > (int)obj ? 1 : 0;
                        break;
                    case ParticleViewerMode.PlayOnAwake:
                        count += particleInfo.PlayOnAwake == (bool)obj ? 1 : 0;
                        break;
                    case ParticleViewerMode.Looping:
                        count += particleInfo.Looping == (bool)obj ? 1 : 0;
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