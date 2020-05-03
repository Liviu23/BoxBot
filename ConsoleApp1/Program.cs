using BoxBot.Entities;
using BoxBot;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Discord.WebSocket;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var box = new Box(Box.GetEssentials().BuildServiceProvider());
            await AddCommandsAsync(box);
            box.Config.ClientType = ClientType.Socket;
            box.Config.DiscordToken = GetToken("token.txt");
            box.SetDiscordSocketConfig(new DiscordSocketConfig()
            {
                WebSocketProvider = Discord.Net.Providers.WS4Net.WS4NetProvider.Instance,
                LogLevel = Discord.LogSeverity.Info
            });

            box.StartAsync();
            while(true)
            {
                switch(Console.ReadLine())
                {
                    case "exit":
                        box.Stop();
                        return;
                }
            }
        }

        static async Task AddCommandsAsync(Box box) => await box.AddCommandModulesAsync(Assembly.GetExecutingAssembly());

        static string GetToken(string path) => File.ReadAllText(path);
    }
}
