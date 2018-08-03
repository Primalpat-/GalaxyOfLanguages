﻿using GalaxyOfLanguages.Logic.Logging.Messages;

namespace GalaxyOfLanguages.Logic.Logging
{
    public class LogMessageFactory
    {
        public ILogMessage CreateLogMessage(string text)
        {
            var message = (ILogMessage) new LogMessage(text);
            message = new Timestamp(message);
            return message;
        }

        public ILogMessage CreateLogMessage(Discord.LogMessage discordLogMessage)
        {
            var message = (ILogMessage) new LogMessage(discordLogMessage.Message);
            message = new Source(message, discordLogMessage.Source);
            message = new Timestamp(message);
            return message;
        }
    }
}