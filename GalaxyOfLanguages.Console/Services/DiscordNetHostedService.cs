using System;
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
        private readonly DiscordSocketClient _client;
        private readonly DiscordNetLogger _discordLogger;
        private readonly JoinedGuild _joinedGuild;
        private readonly MessageReceived _messageReceived;

        public DiscordNetHostedService(AppConfig config, ILogger<DiscordNetHostedService> logger, LogMessageFactory messageFactory,
            DiscordSocketClient client, DiscordNetLogger discordLogger, JoinedGuild joinedGuild, MessageReceived messageReceived)
        {
            _config = config;
            _logger = logger;
            _messageFactory = messageFactory;
            _client = client;
            _discordLogger = discordLogger;
            _joinedGuild = joinedGuild;
            _messageReceived = messageReceived;

            SetClientEvents();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var message = _messageFactory.CreateLogMessage("Starting...");
            _logger.LogInformation(message.Display());

            try
            {
                await _client.LoginAsync(TokenType.Bot, _config.Discord.BotToken);
                await _client.StartAsync();
                await _client.SetGameAsync("with code | *help");
            }
            catch (Exception ex)
            {
                var critical = _messageFactory.CreateLogMessage(ex);
                _logger.LogCritical(critical.Display());
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var message = _messageFactory.CreateLogMessage("Stopping...");
            _logger.LogInformation(message.Display());

            try
            {
                await _client.StopAsync();
            }
            catch (Exception ex)
            {
                var critical = _messageFactory.CreateLogMessage(ex);
                _logger.LogCritical(critical.Display());
            }
        }

        private void SetClientEvents()
        {
            _client.Log += _discordLogger.Log;
            _client.JoinedGuild += (guild) => Task.Run(() => _joinedGuild.Join(guild));
            _client.MessageReceived += (message) => Task.Run(() => _messageReceived.ReceiveMessage(message));

            RegisterObservers();
        }

        private void RegisterObservers()
        {
            var joinedObserver = new JoinedObserver(_joinedGuild);
            var helpObserver = new HelpObserver(_messageReceived);
            var translationObserver = new TranslationObserver(_messageReceived, _config.Translator.ApiKey);
        }
    }
}