using System;

namespace GalaxyOfLanguages.Logic.Logging
{
    public class Timestamp : LogMessageDecorator
    {
        public Timestamp(LogMessage message) : base(message)
        {
        }

        public override string Display()
        {
            return $"{DateTime.Now,-19} {Message.Display()}";
        }
    }
}