using System.Collections.Generic;
using LitJson;
using System.IO;
using System.Linq;

namespace AssetViewer
{
    public class HealthConfig : Singleton<HealthConfig>
    {
        private readonly string Prefix = "HealthConfig-";
        private readonly string Extension = ".json";

        public class TextureJson
        {
            public int ReadWrite;
            public int MipMap;
            public int Resolution;
            public int ResolutionCount;
            public int Trilinear;
        }

        public class ParticleJson
        {
            public int MaxParticle;
            public int MaxParticleCount;
        }

        public class ModeConfig
        {
            public string ModeName;
            public bool Enable;
            public string Tip;
            public int ConfigValue;
            public List<object> Condition;
        }

        public class WinTypeConfig
        {
            public string WinTypeName;
            public List<ModeConfig> ModeConfig;
        }

        public class ConfigJson
        {
            public string Name;
            public List<WinTypeConfig> WinTypeConfig;
            //public TextureJson Texture;
            //public ParticleJson Particle;

            public ModeConfig GetModeConfig(string winTypeConfigName, string modeConfigName)
            {
                WinTypeConfig winTypeConfig = WinTypeConfig.Find(config => config.WinTypeName == winTypeConfigName);
                if (winTypeConfig != null)
                {
                    return winTypeConfig.ModeConfig.Find(config => config.ModeName == modeConfigName);
                }
                return null;
            }
        }

        public Dictionary<string, ConfigJson> HealthConfigDic;

        public override void Init()
        {
            if (HealthConfigDic != null)
            {
                HealthConfigDic.Clear();
            }
            else
            {
                HealthConfigDic = new Dictionary<string, ConfigJson>();
            }

            InitFromFile();
        }

        public void InitFromFile()
        {
            string[] directories = Directory.GetFiles(OverviewConfig.HealthConfigPath);
            foreach (string directory in directories)
            {
                string fileName = Path.GetFileNameWithoutExtension(directory);
                string fileExt = Path.GetExtension(directory);

                if (fileName.StartsWith(Prefix) && fileExt == Extension)
                {
                    ConfigJson configJson = PreseFromFile(directory);
                    AddConfig(configJson.Name, configJson);
                }
            }
        }

        public string[] GetHealthConfigName()
        {
            return HealthConfigDic.Select(z => z.Value.Name).ToArray();
        }

        public ConfigJson GetConfig(string configName)
        {
            if (!HealthConfigDic.ContainsKey(configName))
            {
                return null;
            }
            return HealthConfigDic[configName];
        }

        public void AddConfig(string name, ConfigJson configJson)
        {
            if (!HealthConfigDic.ContainsKey(name))
            {
                HealthConfigDic[name] = configJson;
            }
        }

        public ConfigJson ParseFromString(string jsonString)
        {
            return JsonMapper.ToObject<ConfigJson>(jsonString);
        }

        public ConfigJson PreseFromFile(string path)
        {
            return ParseFromString(File.ReadAllText(path));
        }
    }
}
