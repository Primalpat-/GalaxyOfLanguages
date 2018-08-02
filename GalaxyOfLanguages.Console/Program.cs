using Discord;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Z.Core.Extensions;

namespace GalaxyOfLanguages.Console
{
    public class Program
    {
        public static IConfiguration Configuration { get; set; }

        public static IServiceProvider ServiceProvider { get; set; }
        private static ILogger<Program> _logger;

        public static async Task Main(string[] args = null)
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration((config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("hostsettings.json", optional: true);
                    config.AddEnvironmentVariables(prefix: "PREFIX_");
                });

            var startup = new Startup(host);
            startup.ConfigureApp();
            startup.ConfigureServices();
            startup.ConfigureLogging();

            await host.RunConsoleAsync();

            //var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //var launch = Environment.GetEnvironmentVariable("LAUNCH_PROFILE");

            //if (env.IsNullOrWhiteSpace())
            //    env = "Development";

            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //    .AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true)
            //    .AddEnvironmentVariables();

            //if (env == "Development")
            //    builder.AddUserSecrets<Program>();

            //Configuration = builder.Build();

            //// Create a service collection and configure our depdencies
            //var serviceCollection = new ServiceCollection();
            //ConfigureServices(serviceCollection);

            //// Build the our IServiceProvider and set our static reference to it
            //ServiceProvider = serviceCollection.BuildServiceProvider();



            ////var test = ServiceProvider.GetService<AppSettings>();
            //var test2 = Configuration["AppSettings:MyTestValue"];
            //var test3 = Configuration["Discord:Production-Bot-Token"];


            //// Enter the applicaiton.. (run!)
            //ServiceProvider.GetService<Application>().Run();



            //var test = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            //var services = (IServiceCollection)new ServiceCollection();
            //var startup = new Startup();
            //startup.ConfigureServices(services);
            //ServiceProvider = services.BuildServiceProvider();

            //_logger = ServiceProvider.GetService<ILoggerFactory>()
            //                            .CreateLogger<Program>();

            //_logger.LogDebug("Logger is working!");

            //var service = ServiceProvider.GetService<ISettings>();
            //service.MyServiceMethod();

            //new Program().Start()
            //             .GetAwaiter()
            //             .GetResult();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Make configuration settings available
            //services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            //services.AddSingleton<IConfiguration>(Configuration);

            //var appSettings = new AppSettings();

            //Configuration.GetSection("AppSettings").Bind(appSettings);

            //// Some libraries may still rely on web context type stuff..
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddTransient<IViewRenderService, ViewRenderService>();
            //services.AddTransient<ICurrentUserService, CurrentUserService>();

            //var appConnStr = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "WindowsConnStr" : "LinuxConnStr";
            //services.AddDbContext<MyDbContext>(options =>
            //{
            //    options.UseSqlServer(Configuration.GetConnectionString(appConnStr));
            //}, ServiceLifetime.Scoped);

            //// Repositories
            //services.AddScoped(typeof(IRepository<>), typeof(DomainRepository<>));

            //// Add AutoMapper
            //var config = new AutoMapper.MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile(new AutoMapperProfile());
            //});

            //var mapper = config.CreateMapper();
            //services.AddSingleton(mapper);

            //// Add caching
            //services.AddMemoryCache();

            //// Add logging            
            //services.AddLogging(builder =>
            //{
            //    builder.AddConfiguration(Configuration.GetSection("Logging"))
            //        .AddConsole()
            //        .AddDebug();
            //});

            // Add Application 
            //services.AddTransient<Application>();
        }

        public async Task Start()
        {
            //var client = new DiscordSocketClient();

            //client.Log += Logger;

            //var config = ServiceProvider.GetService<IConfigurationRoot>();
            //await client.LoginAsync(TokenType.Bot, config["Discord:Production-Bot-Token"]);
            //await client.StartAsync();

            //var messageReceived = new MessageReceived();
            //var translationResponder = new TranslationResponder(messageReceived, config["Microsoft:Translation-Api-Key"]);

            //client.MessageReceived += (message) => Task.Run(() => messageReceived.ReceiveMessage(message));

            //_client = new DiscordSocketClient();

            //_client.Log += Logger;

            //await _client.LoginAsync(TokenType.Bot, Configuration["discord.net-token"]);
            //await _client.StartAsync();

            //_messageReceivedHandler = new MessageReceived();
            //var translationResponder = new TranslationResponder(_messageReceivedHandler, Configuration["translation-api-key"]);

            //_client.MessageReceived += (message) => Task.Run(() => _messageReceivedHandler.ReceiveMessage(message));

            await Task.Delay(-1);
        }

        private static Task Logger(LogMessage message)
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