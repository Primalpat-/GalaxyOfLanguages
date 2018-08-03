using System;
using Discord.WebSocket;
using GalaxyOfLanguages.Logic.DiscordResponders;
using GalaxyOfLanguages.Logic.DiscordResponders.Behaviors;
using Z.Core.Extensions;

namespace GalaxyOfLanguages.Logic.DiscordEvents.EventObservers
{
    public class HelpObserver : IObserver<SocketMessage>
    {
        private readonly IDisposable _unsubscriber;

        public HelpObserver(IObservable<SocketMessage> provider)
        {
            if (provider.IsNotNull())
                _unsubscriber = provider.Subscribe(this);
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
            responder.SetResponseBehavior(new HelpBehavior(message));
            responder.Respond();
        }

        public virtual void Unsubscribe()
        {
            _unsubscriber.Dispose();
        }
    }
}
