using Newtonsoft.Json;

namespace GalaxyOfLanguages.Logic.TranslationApi.Microsoft.Models
{
    [JsonObject]
    public class MicrosoftSupportedLanguage
    {
        public string Code { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("nativeName")]
        public string NativeName { get; set; }
        [JsonProperty("dir")]
        public string Direction { get; set; }
    }
}
