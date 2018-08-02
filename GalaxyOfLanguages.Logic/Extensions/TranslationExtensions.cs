using System;
using System.Collections.Generic;
using System.Linq;
using GalaxyOfLanguages.Logic.TranslationApi.Models;

namespace GalaxyOfLanguages.Logic.Extensions
{
    public static class TranslationExtensions
    {
        public static string GetTranslation(this List<Translation> translations, Language language)
        {
            return translations.Single(t => string.Compare(t.LanguageCode,
                                                           language.Code,
                                                           StringComparison.OrdinalIgnoreCase) == 0)
                               .Text;
        }
    }
}
