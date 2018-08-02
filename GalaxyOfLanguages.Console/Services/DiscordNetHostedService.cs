using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using GalaxyOfLanguages.Console.Configuration;
using GalaxyOfLanguages.Logic;
using GalaxyOfLanguages.Logic.EventResponders;
using GalaxyOfLanguages.Logic.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GalaxyOfLanguages.Console.Services
{
    public class DiscordNetHostedService : IHostedService
    {
        private readonly AppConfig _config;
        private readonly ILogger _logger;
        private readonly DiscordSocketClient _client;

        public DiscordNetHostedService(AppConfig config, ILogger<DiscordNetHostedService> logger)
        {
            _config = config;
            _logger = logger;
            _client = new DiscordSocketClient();

            SetClientEvents();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var message = new SimpleLogMessage("Starting...");
            var messageWithTimestamp = new Timestamp(message);

            _logger.LogInformation(messageWithTimestamp.Display());

            //TODO Handle connection errors, and log them
            await _client.LoginAsync(TokenType.Bot, _config.Discord.BotToken);
            await _client.StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var message = new SimpleLogMessage("Stopping...");
            var messageWithTimestamp = new Timestamp(message);

            _logger.LogInformation(messageWithTimestamp.Display());

            await _client.StopAsync();
        }

        private void SetClientEvents()
        {
            _client.Log += Logger;

            var messageReceived = new MessageReceived();
            var translationResponder = new TranslationResponder(messageReceived, _config.Translator.ApiKey);
            _client.MessageReceived += (message) => Task.Run(() => messageReceived.ReceiveMessage(message));
        }

        private static Task Logger(Discord.LogMessage message)
        {
            var cc = System.Console.ForegroundColor;
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    System.Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    System.Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            System.Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message}");
            System.Console.ForegroundColor = cc;

            return Task.CompletedTask;
        }
    }
}