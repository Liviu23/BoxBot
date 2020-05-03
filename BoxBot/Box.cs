using BoxBot.Core;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using static BoxBot.Entities.HandlingDelegates;

namespace BoxBot
{
    public class Box
    {
        private Bot Bot { get; set; }
        public IConfiguration Config { get; private set; }

        public Box(ServiceProvider provider)
        {
            Bot = ActivatorUtilities.GetServiceOrCreateInstance<Bot>(provider);
            Config = Bot.Configuration;
        }

        public async Task StartAsync() => await Bot.StartAsync();
        public void Stop() => Bot.Stop();
        public void Stop(int delay) => Bot.Stop(delay);
        public void Stop(TimeSpan delay) => Bot.Stop(delay);
        public async Task Restart() => await Bot.Restart();


        public void SetDiscordSocketConfig(DiscordSocketConfig config) => Bot.DiscordSocketConfig = config;
        public async Task AddCommandModulesAsync(Assembly assembly) => await Bot.DiscordConnection.AddModulesAsync(assembly);
        public async Task AddCommandModulesAsync(List<Assembly> assemblies) => await Bot.DiscordConnection.AddModulesAsync(assemblies);
        public void SetTypeReaders(AddTypeReaders func) => Bot.DiscordConnection.SetTypeReaders(func);
        public void SetOnMessageRecieved(OnMessageRecieved func) => Bot.DiscordConnection.SetOnMessageRecieved(func);
        public void SetOnExceptionCatched(OnExceptionCatched func) => Bot.DiscordConnection.SetOnExceptionCatched(func);
        public void SetOnCommandExecuted(OnCommandExecuted func) => Bot.DiscordConnection.SetOnCommandExecuted(func);


        public static IServiceCollection GetEssentials(bool addDefaultImplementations = true)
        {
            var services = new ServiceCollection().AddDiscordEssentials();
            if (addDefaultImplementations)
                services.AddDefaults();
            return services;
        }
    }
}
