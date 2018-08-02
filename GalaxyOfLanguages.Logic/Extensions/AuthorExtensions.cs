using Discord.WebSocket;
using Z.Core.Extensions;

namespace GalaxyOfLanguages.Logic.Extensions
{
    public static class AuthorExtensions
    {
        public static string Name(this SocketUser author)
        {
            var guildAuthor = author as SocketGuildUser;

            if (guildAuthor.IsNull())
                return author.Username;

            return guildAuthor.Nickname ?? guildAuthor.Username;
        }
    }
}
