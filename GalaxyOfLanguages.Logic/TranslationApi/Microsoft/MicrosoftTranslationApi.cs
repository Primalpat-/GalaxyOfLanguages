using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GalaxyOfLanguages.Logic.TranslationApi.Microsoft.Models;
using GalaxyOfLanguages.Logic.TranslationApi.Models;
using GalaxyOfLanguages.Logic.TranslationApi.Strategy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GalaxyOfLanguages.Logic.TranslationApi.Microsoft
{
    public class MicrosoftTranslationApi : ITranslationApi
    {
        private const string GetSupportedLanguagesUri = "https://api.cognitive.microsofttranslator.com/languages?api-version=3.0&scope=translation";
        private const string GetTranslationBaseUri = "https://api.cognitive.microsofttranslator.com/translate?api-version=3.0";

        public async Task<List<Language>> SupportedLanguages()
        {
            var result = new List<Language>();

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri(GetSupportedLanguagesUri);

                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                var parsedBody = JObject.Parse(responseBody);
                var rawLanguages = parsedBody["translation"].Children().ToList();
                foreach (var rawLanguage in rawLanguages)
                {
                    var propertyLanguage = rawLanguage as JProperty;
                    if (propertyLanguage == null)
                        continue;

                    var deserializedLanguage = propertyLanguage.Value.ToObject<MicrosoftSupportedLanguage>();

                    var language = new Language
                    {
                        Code = propertyLanguage.Name,
                        Name = deserializedLanguage.Name
                    };

                    result.Add(language);
                }

                return result;
            }
        }

        public async Task<List<Translation>> Translate(string translationApiKey, string textToTranslate, Language languageFrom, List<Language> languagesTo)
        {
            var result = new List<Translation>();

            var params_ = $"&from={languageFrom.Code}";
            foreach (var languageTo in languagesTo)
                params_ += $"&to={languageTo.Code}";

            var body = new object[] { new { Text = textToTranslate } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(GetTranslationBaseUri + params_);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", translationApiKey);

                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                var deserializedResult = JsonConvert.DeserializeObject<List<MicrosoftTranslationResult>>(responseBody);
                foreach(var translation in deserializedResult[0].Translations)
                    result.Add(new Translation
                    {
                        LanguageCode = translation.To,
                        Text = translation.Text
                    });

                return result;
            }
        }
    }
}