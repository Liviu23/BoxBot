using BoxBot.Core;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using static BoxBot.Entities.HandlingDelegates;

namespace BBox.Discord
{
    internal class CommandHandler
    {
        private CommandService commandService;
        private IClientManager clientManager;
        private IServiceProvider services;
        public OnMessageRecieved handleCommandAsync;
        public OnCommandExecuted commandExecuted;
        public OnExceptionCatched commandServiceLog;
        public AddTypeReaders addTypeReaders;

        public CommandHandler(IClientManager clientManager, IServiceProvider services)
        {
            this.clientManager = clientManager;
            this.services = services;
            commandService = new CommandService();

            handleCommandAsync = new OnMessageRecieved(HandleCommandAsync);
            commandExecuted = new OnCommandExecuted(OnCommandExecuted);
            commandServiceLog = new OnExceptionCatched(LogAsync);
        }

        public void Initialize()
        {
            addTypeReaders?.Invoke(commandService);
            (clientManager.Client as BaseSocketClient).MessageReceived += (s) => handleCommandAsync?.Invoke(s, clientManager.Client, commandService, services);
            commandService.CommandExecuted += (arg1, arg2, arg3) => commandExecuted?.Invoke(arg1, arg2, arg3);
            commandService.Log += (arg) => commandServiceLog?.Invoke(arg);
        }

        public async Task AddCommandModulesAsync(List<Assembly> assemblies)
        {
            foreach (var x in assemblies)
                await AddCommandModulesAsync(x);
        }
        public async Task AddCommandModulesAsync(Assembly assembly) => await commandService.AddModulesAsync(assembly, services);


        #region Defalut functions for delegates
        private async Task HandleCommandAsync(SocketMessage s, IDiscordClient client, CommandService commandService, IServiceProvider services)
        {
            if (!(s is SocketUserMessage msg))
            {
                return;
            }

            var argPos = 0;
            if (msg.HasMentionPrefix(clientManager.Client.CurrentUser, ref argPos))
            {
                ICommandContext context = null;
                if (client is DiscordSocketClient socketClient)
                    context = new SocketCommandContext(socketClient, msg);
                else if (client is DiscordShardedClient shardedClient)
                    context = new ShardedCommandContext(shardedClient, msg);

                if (!commandService.Search(context, argPos).IsSuccess)
                {
                    await context.Channel.SendMessageAsync("Command not found");
                    return;
                }

                await commandService.ExecuteAsync(context, argPos, services).ConfigureAwait(false);
            }
        }

        private async Task OnCommandExecuted(Optional<CommandInfo> arg1, ICommandContext arg2, IResult arg3)
        {
            if (arg3.IsSuccess)
                return;
            string message = arg3.ErrorReason;
            if (!string.IsNullOrEmpty(message))
                await arg2.Channel.SendMessageAsync(message);
        }

        private async Task LogAsync(LogMessage arg)
        {
            if (arg.Exception is CommandException cmdEx)
            {
                await cmdEx.Context.Channel.SendMessageAsync($"**Something went extremely bad:**\n{cmdEx.Message}");
            }
        }

        #endregion   
    }
}
