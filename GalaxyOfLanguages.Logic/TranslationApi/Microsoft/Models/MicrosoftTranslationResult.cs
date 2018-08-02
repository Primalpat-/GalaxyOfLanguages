using System.Collections.Generic;
using Newtonsoft.Json;

namespace GalaxyOfLanguages.Logic.TranslationApi.Microsoft.Models
{
    [JsonObject]
    public class MicrosoftTranslationResult
    {
        [JsonProperty("detectedLanguage")]
        public MicrosoftDetectedLanguage Detected { get; set; }
        [JsonProperty("translations")]
        public List<MicrosoftTranslation> Translations { get; set; }
    }

    [JsonObject]
    public class MicrosoftDetectedLanguage
    {
        [JsonProperty("language")]
        public string Language { get; set; }
        [JsonProperty("score")]
        public float Score { get; set; }
    }

    [JsonObject]
    public class MicrosoftTranslation
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("to")]
        public string To { get; set; }
    }
}
