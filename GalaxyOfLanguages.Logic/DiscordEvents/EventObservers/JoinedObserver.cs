using System;
using Discord.WebSocket;
using GalaxyOfLanguages.Logic.DiscordResponders;
using GalaxyOfLanguages.Logic.DiscordResponders.Behaviors;
using Z.Core.Extensions;

namespace GalaxyOfLanguages.Logic.DiscordEvents.EventObservers
{
    public class JoinedObserver : IObserver<SocketGuild>
    {
        private readonly IDisposable _unsubscriber;

        public JoinedObserver(IObservable<SocketGuild> provider)
        {
            if (provider.IsNotNull())
                _unsubscriber = provider.Subscribe(this);
        }

        public void OnCompleted()
        {
            //TODO
            Console.WriteLine("Additional joins will not be processed.");
        }

        public void OnError(Exception error)
        {
            //TODO
            throw error;
        }

        public void OnNext(SocketGuild guild)
        {
            var responder = new DiscordResponder();
            responder.SetResponseBehavior(new JoinedBehavior(guild));
            responder.Respond();
        }

        public virtual void Unsubscribe()
        {
            _unsubscriber.Dispose();
        }
    }
}