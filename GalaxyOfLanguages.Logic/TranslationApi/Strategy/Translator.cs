using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyOfLanguages.Logic.TranslationApi.Models;

namespace GalaxyOfLanguages.Logic.TranslationApi.Strategy
{
    public class Translator
    {
        private ITranslationApi _translationApi;

        public void SetTranslationApi(ITranslationApi translationApi)
        {
            _translationApi = translationApi;
        }

        public Task<List<Language>> GetSupportedLanguages()
        {
            return _translationApi.SupportedLanguages();
        }

        public Task<List<Translation>> GetTranslations(string translationApiKey,
                                                       string textToTranslate,
                                                       Language languageFrom,
                                                       List<Language> languagesTo)
        {
            return _translationApi.Translate(translationApiKey, textToTranslate, languageFrom, languagesTo);
        }
    }
}