using System;

namespace GalaxyOfLanguages.Logic.Logging
{
    public class SimpleLogMessage : LogMessage
    {
        public SimpleLogMessage(string text)
        {
            Text = text;
        }

        public override string Display()
        {
            return Text;
        }
    }
}