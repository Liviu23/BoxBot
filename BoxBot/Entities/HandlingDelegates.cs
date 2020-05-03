using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace BoxBot.Entities
{
    public class HandlingDelegates
    {
        public delegate Task OnMessageRecieved(SocketMessage s, IDiscordClient client, CommandService commandService, IServiceProvider services);
        public delegate Task OnCommandExecuted(Optional<CommandInfo> arg1, ICommandContext arg2, IResult arg3);
        public delegate Task OnExceptionCatched(LogMessage arg);
        public delegate void AddTypeReaders(CommandService cmdService);
    }
}
