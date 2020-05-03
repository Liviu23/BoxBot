using BoxBot.Entities;
using BoxBot;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var box = new Box(Box.GetEssentials().BuildServiceProvider());
            box.Config.ClientType = ClientType.Socket;
            box.Config.DiscordToken = GetToken("token.txt");
            
        }


        static string GetToken(string path) => File.ReadAllText(path);
    }
}
