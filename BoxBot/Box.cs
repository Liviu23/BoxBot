using BoxBot.Core;
using Microsoft.Extensions.DependencyInjection;

namespace BoxBot
{
    public class Box
    {
        public Bot Bot { get; set; }
        public IConfiguration Config { get; private set; }

        public Box(ServiceProvider provider)
        {
            Bot = ActivatorUtilities.GetServiceOrCreateInstance<Bot>(provider);
            Config = Bot.Configuration;
        }


        public static IServiceCollection GetEssentials(bool addDefaultImplementations = true)
        {
            var services = new ServiceCollection().AddDiscordEssentials();
            if (addDefaultImplementations)
                services.AddDefaults();
            return services;
        }
    }
}
