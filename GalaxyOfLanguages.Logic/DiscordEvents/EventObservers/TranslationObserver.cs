﻿using System;
using Discord.WebSocket;
using GalaxyOfLanguages.Logic.DiscordResponders;
using GalaxyOfLanguages.Logic.DiscordResponders.Behaviors;
using Z.Core.Extensions;

namespace GalaxyOfLanguages.Logic.DiscordEvents.EventObservers
{
    public class TranslationObserver : IObserver<SocketMessage>
    {
        private readonly IDisposable _unsubscriber;
        private readonly string _translationApiKey;

        public TranslationObserver(IObservable<SocketMessage> provider, string translationApiKey)
        {
            if (provider.IsNotNull())
                _unsubscriber = provider.Subscribe(this);

            _translationApiKey = translationApiKey;
        }

        public void OnCompleted()
        {
            //TODO
            Console.WriteLine("Additional messages will not be processed.");
        }

        public void OnError(Exception error)
        {
            //TODO
            throw error;
        }

        public void OnNext(SocketMessage message)
        {
            var responder = new DiscordResponder();
            responder.SetResponseBehavior(new TranslationBehavior(message, _translationApiKey));
            responder.Respond();
        }

        public virtual void Unsubscribe()
        {
            _unsubscriber.Dispose();
        }
    }
}
