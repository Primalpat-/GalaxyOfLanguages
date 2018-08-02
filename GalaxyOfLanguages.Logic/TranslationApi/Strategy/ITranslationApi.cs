using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyOfLanguages.Logic.TranslationApi.Models;

namespace GalaxyOfLanguages.Logic.TranslationApi.Strategy
{
    public interface ITranslationApi
    {
        string SupportedLanguagesUrl();
        Task<List<Language>> SupportedLanguages();
        Task<List<Translation>> Translate(string translationApiKey, 
                                          string textToTranslate, 
                                          Language languageFrom, 
                                          List<Language> languagesTo);
    }
}