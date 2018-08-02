using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
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
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(SocketMessage value)
        {
            throw new NotImplementedException();
        }
    }
}
