using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace GalaxyOfLanguages.Logic.DiscordResponders.Behaviors
{
    public class HelpBehavior : IResponseBehavior
    {
        private readonly SocketMessage _message;

        public HelpBehavior(SocketMessage message)
        {
            _message = message;
        }

        public async Task SendResponse()
        {
            if (FilterMessage())
                return;

            await _message.Channel
                          .SendMessageAsync($"```asciidoc\r\n" +
                                            $"= Galaxy-Of-Languages Bot =\r\n" +
                                            $"written by discord user Primalpat#9990\r\n\r\n" +
                                            $"[Thank you for adding the galaxy-of-languages bot to your server!]\r\n\r\n" +
                                            $"This bot works with \"families\" of related channels.  Once configured, it will take the base language for the channel " +
                                            $"(denoted by its 2 digit language code) it is receiving a message in, translate it, and then send those translations " +
                                            $"to the rest of the related channels.\r\n\r\n" +
                                            $"For a more seemless experience, it makes it look as if translations were written by the user who sent the original message.  " +
                                            $"To accomplish this the bot makes use of *webhooks*.  A webhook will need to be created in each of the " +
                                            $"related channels.\r\n\r\n" +
                                            $"[Example:]\r\n" +
                                            $"You have a channel called \"general\" where everybody talks in english.  You want to translate what is written there " +
                                            $"to french and german, and also have a way for french and german speakers to be translated to english.\r\n\r\n" +
                                            $"To accomplish this, you would need to create 2 new channels: one named \"general-fr\" and the other \"general-de\".  " +
                                            $"You could also rename \"general\" to \"general-en\", but it is not required.  Channels without a language code are assumed english.  " +
                                            $"Next you would need to create a webhook for each of the channels, the name of which should not matter.  To create a webhook, edit the " +
                                            $"channel you wish to create it for, go to the \"Webhooks\" tab, and press the \"Create Webhook\" button." +
                                            $"\r\n```");
        }

        private bool FilterMessage()
        {
            if (_message.Source == MessageSource.Bot)
                return true;

            if (_message.Source == MessageSource.Webhook)
                return true;

            if (!_message.Content.StartsWith("*help"))
                return true;

            return false;
        }
    }
}