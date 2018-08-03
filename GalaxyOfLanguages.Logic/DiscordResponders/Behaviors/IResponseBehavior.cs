using System.Threading.Tasks;

namespace GalaxyOfLanguages.Logic.DiscordResponders.Behaviors
{
    public interface IResponseBehavior
    {
        Task SendResponse();
    }
}