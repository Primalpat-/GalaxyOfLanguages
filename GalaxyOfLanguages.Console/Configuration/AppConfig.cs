namespace GalaxyOfLanguages.Console.Configuration
{
    public class AppConfig
    {
        public DiscordConfig Discord { get; set; } = new DiscordConfig();
        public TranslatorConfig Translator { get; set; } = new TranslatorConfig();
    }
}
