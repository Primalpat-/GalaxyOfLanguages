using GalaxyOfLanguages.Logic.TranslationApi.Strategy;

namespace GalaxyOfLanguages.Logic.TranslationApi.Microsoft
{
    public class MicrosoftTranslator : Translator
    {
        public MicrosoftTranslator()
        {
            TranslationApi = new MicrosoftTranslationApi();
        }
    }
}
