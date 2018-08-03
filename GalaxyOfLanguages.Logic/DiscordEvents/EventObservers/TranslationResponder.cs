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
using Z.Core.Extensions;

namespace GalaxyOfLanguages.Logic.DiscordEvents.EventObservers
{
    public class TranslationResponder : IDiscordResponder, IObserver<SocketMessage>
    {
        private readonly IDisposable _unsubscriber;
        private readonly string _translationApiKey;

        public TranslationResponder(IObservable<SocketMessage> provider, string translationApiKey)
        {
            if (provider.IsNotNull())
                _unsubscriber = provider.Subscribe(this);

            _translationApiKey = translationApiKey;
        }

        public void Respond()
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public async void OnNext(SocketMessage message)
        {
            //Do not translate webhook or bot output
            if (message.Source == MessageSource.Bot || message.Source == MessageSource.Webhook)
                return;

            var originChannel = message.Channel as SocketGuildChannel;
            var relatedChannels = GetRelatedChannels(originChannel);

            var translator = new MicrosoftTranslator();
            var translations = await translator.GetTranslations(_translationApiKey, 
                                                                message.Content, 
                                                                originChannel.Language(), 
                                                                relatedChannels.Select(c => c.Language()).ToList());

            foreach (var destinationChannel in relatedChannels)
                await SendTranslation(destinationChannel, 
                                      translations.GetTranslation(destinationChannel.Language()),
                                      message);
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

        private async Task SendTranslation(SocketTextChannel channel, string translatedText, SocketMessage originalMessage)
        {
            try
            {
                var webhookClient = await GetWebhookClient(channel);
                await webhookClient.SendMessageAsync(text: translatedText,  
                                                     embeds: originalMessage.Embeds.ToArray(), 
                                                     username: $"{originalMessage.Author.Name()} ({channel.Language().Code})",
                                                     avatarUrl: originalMessage.Author.GetAvatarUrl());
            }
            catch (MissingWebhookException ex)
            {
                await originalMessage.Channel.SendMessageAsync($"{ex.Message}");
            }
        }

        private async Task<DiscordWebhookClient> GetWebhookClient(SocketTextChannel channel)
        {
            var webhooks = await channel.GetWebhooksAsync();
            var targetWebhook = webhooks.FirstOrDefault(w => w.Name.InsensitiveContains("Translator"));

            if (targetWebhook.IsNull())
                targetWebhook = webhooks.FirstOrDefault();

            if (targetWebhook.IsNull())
                throw new MissingWebhookException($"```autohotkey" +
                                                  $"{channel.Name} does not have a webhook configured" +
                                                  $"```");

            return new DiscordWebhookClient(targetWebhook.Id, targetWebhook.Token);
        }

        public virtual void Unsubscribe()
        {
            _unsubscriber.Dispose();
        }


    }
}