using GalaxyOfLanguages.Logic.DiscordResponders.Behaviors;

namespace GalaxyOfLanguages.Logic.DiscordResponders
{
    public class DiscordResponder
    {
        private IResponseBehavior _responseBehavior;

        public void SetResponseBehavior(IResponseBehavior behavior)
        {
            _responseBehavior = behavior;
        }

        public void Respond()
        {
            _responseBehavior.SendResponse();
        }
    }
}