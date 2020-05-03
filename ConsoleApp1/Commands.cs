using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("echo", RunMode = RunMode.Async)]
        [Alias("say")]
        public async Task Echo([Remainder]string msg)
        {
            await ReplyAsync(msg);
        }

        [Command("random", RunMode = RunMode.Async), Alias("rand")]

        public async Task GetRandomNumber(int maxVal, int minVal = 0)
        {
            await ReplyAsync(new Random().Next(minVal, maxVal).ToString());
        }
    }
}
