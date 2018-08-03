using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using GalaxyOfLanguages.Console.Configuration;
using GalaxyOfLanguages.Logic.DiscordEvents.EventObservables;
using GalaxyOfLanguages.Logic.DiscordEvents.EventObservers;
using GalaxyOfLanguages.Logic.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GalaxyOfLanguages.Console.Services
{
    public class DiscordNetHostedService : IHostedService
    {
        private readonly AppConfig _config;
        private readonly ILogger _logger;
        private readonly LogMessageFactory _messageFactory;
        private readonly DiscordNetLogger _discordLogger;
        private readonly DiscordSocketClient _client;

        public DiscordNetHostedService(AppConfig config, ILogger<DiscordNetHostedService> logger, LogMessageFactory messageFactory,
            DiscordNetLogger discordLogger)
        {
            _config = config;
            _logger = logger;
            _messageFactory = messageFactory;
            _discordLogger = discordLogger;
            _client = new DiscordSocketClient();

            SetClientEvents();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var message = _messageFactory.CreateLogMessage("Starting...");
            _logger.LogInformation(message.Display());

            //TODO Handle connection errors, and log them
            await _client.LoginAsync(TokenType.Bot, _config.Discord.BotToken);
            await _client.StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var message = _messageFactory.CreateLogMessage("Stopping...");
            _logger.LogInformation(message.Display());

            await _client.StopAsync();
        }

        private void SetClientEvents()
        {
            _client.Log += _discordLogger.Log;

            var messageReceived = new MessageReceived();
            var translationResponder = new TranslationResponder(messageReceived, _config.Translator.ApiKey);
            _client.MessageReceived += (message) => Task.Run(() => messageReceived.ReceiveMessage(message));
        }
    }
}