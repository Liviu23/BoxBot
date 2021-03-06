﻿using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace BoxBot.Discord.SampleCommands
{
    public class SocketCommands : ModuleBase<SocketCommandContext>
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
