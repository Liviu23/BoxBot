using BoxBot;
using BoxBot.Core;
using BoxBot.Entities;
using BoxBot.Implementations;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = SetServices();
            Box box = new Box(services.BuildServiceProvider());
            
            try
            {
                ConfigureBot(box.Bot);
                box.Bot.ExceptionCatched += Bot_ExceptionCatched;

                // Start the bot
                // Exit when bot is stopped manually
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
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
                if (box.Bot != null)
                    box.Bot.Stop();
            }
        }

        private static void Bot_ExceptionCatched(object sender, Exception e)
        {
            Console.WriteLine($"{e.Message}\n{e.StackTrace}");
        }
        
        private static void ConfigureBot(Bot bot)
        {
            bot.Configuration.ClientType = ClientType.Socket;
            //bot.Configuration.TokenType = Discord.TokenType.Bot;
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
