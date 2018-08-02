using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyOfLanguages.Logic.TranslationApi.Models;

namespace GalaxyOfLanguages.Logic.TranslationApi.Strategy
{
    public abstract class Translator
    {
        public ITranslationApi TranslationApi;

        public Task<List<Language>> GetSupportedLanguages()
        {
            return TranslationApi.SupportedLanguages();
        }

        public Task<List<Models.Translation>> GetTranslations(string translationApiKey,
                                                              string textToTranslate,
                                                              Language languageFrom,
                                                              List<Language> languagesTo)
        {
            return TranslationApi.Translate(translationApiKey, textToTranslate, languageFrom, languagesTo);
        }
    }
}
