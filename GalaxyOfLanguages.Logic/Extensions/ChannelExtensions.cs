using System;
using System.Linq;
using Discord.WebSocket;
using GalaxyOfLanguages.Logic.TranslationApi;
using GalaxyOfLanguages.Logic.TranslationApi.Models;
using Z.Collections.Extensions;
using Z.Core.Extensions;

namespace GalaxyOfLanguages.Logic.Extensions
{
    public static class ChannelExtensions
    {
        /// <summary>
        /// Given a channel, will parse out the primary language code based on the channel name.
        /// </summary>
        public static Language Language(this SocketGuildChannel channel)
        {
            var supportedLanguages = SupportedLanguages.GetInstance()
                                                       .GetLanguages();

            var name = channel.Name;
            var split = name.Split('-');

            if (split.Length > 1)
            {
                var language = supportedLanguages.FirstOrDefault(l =>
                    string.Compare(l.Code, split.Last(), StringComparison.OrdinalIgnoreCase) == 0);

                if (language.IsNotNull())
                    return language;
            }

            return new Language
            {
                Code = "en",
                Name = "English"
            };
        }

        /// <summary>
        /// Given a channel, will remove the language "-en" part and return the base name
        /// </summary>
        public static string BaseName(this SocketGuildChannel channel)
        {
            var supportedLanguages = SupportedLanguages.GetInstance()
                                                       .GetLanguages();

            var name = channel.Name;
            var split = name.Split('-');

            if (split.Length > 1)
            {
                var language = supportedLanguages.FirstOrDefault(l =>
                    string.Compare(l.Code, split.Last(), StringComparison.OrdinalIgnoreCase) == 0);

                if (language.IsNotNull())
                {
                    split = split.Take(split.Length - 1).ToArray();
                    return split.StringJoin('-');
                }
            }

            return name;
        }
    }
}
