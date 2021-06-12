using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DiscordBot
{
    public static class ConfigManager
    {
        private static string _configFile;

        private static string ConfigFile
        {
            get
            {
                if (string.IsNullOrEmpty(_configFile))
                    _configFile = Path.Combine(Environment.CurrentDirectory, "config.yaml");

                return _configFile;
            }
        }

        private static string ReadEggConfig()
        {
            using (StreamReader reader = new StreamReader(ConfigFile))
            {
                return reader.ReadToEnd();
            }
        }
        
        public static EggConfig GetEggConfig()
        {
            var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();

            return deserializer.Deserialize<EggConfig>(ReadEggConfig());
        }
    }
}