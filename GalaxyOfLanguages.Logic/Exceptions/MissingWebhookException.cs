using System;
using System.Collections.Generic;
using System.Text;

namespace GalaxyOfLanguages.Logic.Exceptions
{
    public class MissingWebhookException : Exception
    {
        public MissingWebhookException()
        {
        }

        public MissingWebhookException(string message) : base(message)
        {
        }

        public MissingWebhookException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
