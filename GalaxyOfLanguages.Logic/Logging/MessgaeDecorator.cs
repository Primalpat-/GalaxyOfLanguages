namespace GalaxyOfLanguages.Logic.Logging
{
    public abstract class LogMessageDecorator : LogMessage
    {
        protected LogMessage Message;

        protected LogMessageDecorator(LogMessage message)
        {
            this.Message = message;
        }

        public override string Display()
        {
            return Message.Display();
        }
    }
}
