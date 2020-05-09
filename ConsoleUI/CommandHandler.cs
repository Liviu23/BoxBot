using BoxBot.Core;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class CommandHandler : ICommandHandler
    {
        private CommandService commandService;
        private IClientManager clientManager;
        private IServiceProvider services;

        public CommandHandler(IClientManager clientManager, IServiceProvider services)
        {
            this.clientManager = clientManager;
            this.services = services;
            commandService = new CommandService();
        }

        public async Task Initialize()
        {
            commandService.CommandExecuted += OnCommandExecuted;
            commandService.Log += LogAsync;
            await commandService.AddModulesAsync(Assembly.GetExecutingAssembly(),services);
            
            if (!(clientManager.Client is DiscordSocketClient client))
                return;
            client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            if (!(s is SocketUserMessage msg))
            {
                return;
            }

            var argPos = 0;
            if (msg.HasMentionPrefix(clientManager.Client.CurrentUser, ref argPos))
            {
                ICommandContext context = new SocketCommandContext((clientManager.Client as DiscordSocketClient), msg);

                if (!commandService.Search(context, argPos).IsSuccess)
                {
                    await context.Channel.SendMessageAsync("Command not found! (¯―¯)");
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
    }
}
