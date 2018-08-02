using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.WebSocket;

namespace GalaxyOfLanguages.Logic.EventResponders
{
    public abstract class DiscordResponder
    {
        public IDiscordResponder Responder;

        public void Respond()
        {
            Responder.Respond();
        }

        /// <summary>
        /// All responders will follow the below filter
        /// </summary>
        private static bool FilterMessage(IMessage message)
        {
            if (message.Source == MessageSource.Bot)
                return true;

            if (message.Source == MessageSource.Webhook)
                return true;

            return false;
        }
    }
}
