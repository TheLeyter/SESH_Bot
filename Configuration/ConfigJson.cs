using System.IO;
using Newtonsoft.Json;

namespace DiscordBot
{
    struct ConfigJson
    {
        public string Token { get; set; }
        public string Prefix { get; set; }
        public string GameStatus { get; set; }
    }
    class Configuration
    {
        public ConfigJson conf { get; }
        public Configuration()
        {
            string ConfigStr = File.ReadAllText("Config.json");
            conf = JsonConvert.DeserializeObject<ConfigJson>(ConfigStr);
        }
    }
}
