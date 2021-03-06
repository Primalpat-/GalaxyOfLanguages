﻿using System.Collections.Generic;
using GalaxyOfLanguages.Logic.TranslationApi.Microsoft;
using GalaxyOfLanguages.Logic.TranslationApi.Models;
using GalaxyOfLanguages.Logic.TranslationApi.Strategy;
using Z.Core.Extensions;

namespace GalaxyOfLanguages.Logic.TranslationApi
{
    public class SupportedLanguages
    {
        private static SupportedLanguages _uniqueInstance;
        private readonly List<Language> _supportedLanguages;

        private SupportedLanguages()
        {
            var translator = new Translator();
            translator.SetTranslationApi(new MicrosoftTranslationApi());
            _supportedLanguages = translator.GetSupportedLanguages().Result;
        }

        public static SupportedLanguages GetInstance()
        {
            if (_uniqueInstance.IsNull())
                _uniqueInstance = new SupportedLanguages();

            return _uniqueInstance;
        }

        public List<Language> GetLanguages()
        {
            return _supportedLanguages;
        }
    }
}