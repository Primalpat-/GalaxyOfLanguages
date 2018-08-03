using System;
using System.Collections.Generic;
using Discord.WebSocket;
using GalaxyOfLanguages.Logic.Observable;

namespace GalaxyOfLanguages.Logic.DiscordEvents.EventObservables
{
    public class JoinedGuild : IObservable<SocketGuild>
    {
        private readonly List<IObserver<SocketGuild>> _observers;

        public JoinedGuild()
        {
            _observers = new List<IObserver<SocketGuild>>();
        }

        public IDisposable Subscribe(IObserver<SocketGuild> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);

            return new Unsubscriber<SocketGuild>(_observers, observer);
        }

        public void Join(SocketGuild guild)
        {
            foreach (var observer in _observers)
                observer.OnNext(guild);
        }
    }
}