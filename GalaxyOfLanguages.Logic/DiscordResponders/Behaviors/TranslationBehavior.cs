using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Webhook;
using Discord.WebSocket;
using GalaxyOfLanguages.Logic.Exceptions;
using GalaxyOfLanguages.Logic.Extensions;
using GalaxyOfLanguages.Logic.TranslationApi.Microsoft;
using GalaxyOfLanguages.Logic.TranslationApi.Strategy;
using Z.Core.Extensions;

namespace GalaxyOfLanguages.Logic.DiscordResponders.Behaviors
{
    public class TranslationBehavior : IResponseBehavior
    {
        private readonly SocketMessage _message;
        private readonly string _translationApiKey;

        public TranslationBehavior(SocketMessage message, string translationApiKey)
        {
            _message = message;
            _translationApiKey = translationApiKey;
        }

        public async Task SendResponse()
        {
            if (FilterMessage())
                return;

            var originChannel = _message.Channel as SocketGuildChannel;
            var relatedChannels = GetRelatedChannels(originChannel);

            var translator = new Translator();
            translator.SetTranslationApi(new MicrosoftTranslationApi());
            var translations = await translator.GetTranslations(_translationApiKey,
                                                                _message.Content,
                                                                originChannel.Language(),
                                                                relatedChannels.Select(c => c.Language()).ToList());

            foreach (var destinationChannel in relatedChannels)
                await SendTranslation(destinationChannel,
                                      translations.GetTranslation(destinationChannel.Language()));
        }

        private bool FilterMessage()
        {
            if (_message.Source == MessageSource.Bot)
                return true;

            if (_message.Source == MessageSource.Webhook)
                return true;

            return false;
        }

        private List<SocketTextChannel> GetRelatedChannels(SocketGuildChannel originChannel)
        {
            return originChannel.Guild
                                .TextChannels
                                .Where(c => string.Compare(c.BaseName(),
                                                           originChannel.BaseName(),
                                                           StringComparison.OrdinalIgnoreCase) == 0 &&
                                            string.Compare(c.Language().Code,
                                                           originChannel.Language().Code,
                                                           StringComparison.OrdinalIgnoreCase) != 0)
                                .ToList();
        }

        private async Task SendTranslation(SocketTextChannel channel, string translatedText)
        {
            try
            {
                if (translatedText.IsPunctuation(0))
                    translatedText = $"_ _{translatedText}";

                var webhookClient = await GetWebhookClient(channel);
                await webhookClient.SendMessageAsync(text: translatedText,
                                                     embeds: _message.Embeds.ToArray(),
                                                     username: $"{_message.Author.Name()} ({channel.Language().Code})",
                                                     avatarUrl: _message.Author.GetAvatarUrl());
            }
            catch (MissingWebhookException ex)
            {
                await _message.Channel.SendMessageAsync($"{ex.Message}");
            }
        }

        private async Task<DiscordWebhookClient> GetWebhookClient(SocketTextChannel channel)
        {
            var webhooks = await channel.GetWebhooksAsync();
            var targetWebhook = webhooks.FirstOrDefault(w => w.Name.InsensitiveContains("Translator"));

            if (targetWebhook.IsNull())
                targetWebhook = webhooks.FirstOrDefault();

            if (targetWebhook.IsNull())
                throw new MissingWebhookException($"```diff\r\n" +
                                                  $"- The channel {channel.Name} does not have a webhook configured -\r\n" +
                                                  $"Please create a webhook for it by editing the channel, going to the Webhooks tab, and " +
                                                  $"pressing the Create Webhook button. The name doesn't matter." +
                                                  $"\r\n```");

            return new DiscordWebhookClient(targetWebhook.Id, targetWebhook.Token);
        }
    }
}