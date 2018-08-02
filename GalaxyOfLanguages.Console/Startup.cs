using System;
using GalaxyOfLanguages.Console.Configuration;
using GalaxyOfLanguages.Console.Services;
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
                services.AddTransient(sp => sp.GetService<IOptions<AppConfig>>().Value);

                services.AddScoped<IHostedService, DiscordNetHostedService>();
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
