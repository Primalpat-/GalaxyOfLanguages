using System;
using Discord.WebSocket;
using GalaxyOfLanguages.Console.Configuration;
using GalaxyOfLanguages.Console.Services;
using GalaxyOfLanguages.Logic.DiscordEvents.EventObservables;
using GalaxyOfLanguages.Logic.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GalaxyOfLanguages.Console
{
    public class Startup
    {
        private readonly IHostBuilder HostBuilder;

        public Startup(IHostBuilder hostBuilder)
        {
            HostBuilder = hostBuilder;
        }

        public IHostBuilder ConfigureApp()
        {
            return HostBuilder.ConfigureAppConfiguration((hostContext, config) =>
            {
                config.SetBasePath(Environment.CurrentDirectory);
                config.AddJsonFile("appsettings.json", optional: false);
                config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                config.AddEnvironmentVariables(prefix: "PREFIX_");

                if (hostContext.HostingEnvironment.IsDevelopment())
                    config.AddUserSecrets<Startup>();
            });
        }

        public IHostBuilder ConfigureServices()
        {
            return HostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddLogging();

                services.AddOptions();
                services.Configure<AppConfig>(hostContext.Configuration.GetSection("App"));
                services.AddScoped(sp => sp.GetService<IOptions<AppConfig>>().Value);

                services.AddScoped<IHostedService, DiscordNetHostedService>();
                services.AddTransient<LogMessageFactory>();
                services.AddTransient<DiscordSocketClient>();
                services.AddTransient<DiscordNetLogger>();
                services.AddTransient<JoinedGuild>();
                services.AddTransient<MessageReceived>();
            });
        }

        public IHostBuilder ConfigureLogging()
        {
            return HostBuilder.ConfigureLogging((hostContext, config) =>
            {
                config.AddConsole();
                config.AddDebug();
            });
        }
    }
}