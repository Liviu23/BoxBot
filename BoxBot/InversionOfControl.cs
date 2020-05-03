using BBox.Discord;
using BoxBot.Core;
using BoxBot.Discord;
using BoxBot.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace BoxBot
{
    internal static class InversionOfControl
    {
        public static IServiceCollection AddDiscordEssentials(this IServiceCollection services)
            => services.AddSingleton<IClientManager, ClientManager>()
               .AddSingleton<IDiscordConnection, DiscordConnection>()
               .AddSingleton<DiscordLogger>()
               .AddSingleton<CommandHandler>();

        public static IServiceCollection AddDefaults(this IServiceCollection services)
            => services.AddSingleton<IConfiguration, BotConfiguration>()
               .AddSingleton<IDiscordLogger, ConsoleLogger>();
    }
}
