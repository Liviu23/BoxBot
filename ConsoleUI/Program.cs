using BoxBot;
using BoxBot.Core;
using BoxBot.Entities;
using BoxBot.Implementations;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = SetServices();
            Box box = new Box(services.BuildServiceProvider());

            ConfigureBot(box.Bot);

            // Start the bot
            // Exit when bot is stopped
            box.Bot.StartAsync();
            while (true)
            {
                switch (Console.ReadLine())
                {
                    case "exit":
                        box.Bot.Stop();
                        return;
                }
            }
        }


        private static void ConfigureBot(Bot bot)
        {
            bot.Configuration.ClientType = ClientType.Socket;
            bot.Configuration.DiscordToken = File.ReadAllText("token.txt");
            bot.DiscordSocketConfig = new DiscordSocketConfig()
            {
                WebSocketProvider = Discord.Net.Providers.WS4Net.WS4NetProvider.Instance,
                LogLevel = Discord.LogSeverity.Verbose
            };
        }

        private static IServiceCollection SetServices()
            => Box.GetEssentials(false)
            .AddSingleton<IConfiguration, BotConfiguration>()
            .AddSingleton<IDiscordLogger, ConsoleLogger>()
            .AddSingleton<ICommandHandler, CommandHandler>();

    }
}
