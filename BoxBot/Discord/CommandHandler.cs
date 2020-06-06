using BoxBot.Core;
using BoxBot.Discord.SampleCommands;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace BBox.Discord
{
    internal class CommandHandler : ICommandHandler
    {
        private DiscordSocketClient Client;
        private CommandService commandService;
        private IClientManager clientManager;
        private IServiceProvider services;

        public CommandHandler(IClientManager clientManager, IServiceProvider services)
        {
            this.clientManager = clientManager;
            this.services = services;
            commandService = new CommandService();
        }

        public async Task InitializeAsync()
        {
            if (!(clientManager.Client is DiscordSocketClient))
                throw new Exception("The default command handler requires a socket client. I recomend using your own custom command handler.");
            Client = (DiscordSocketClient)clientManager.Client;

            commandService.CommandExecuted += OnCommandExecuted;
            commandService.Log += LogAsync;
            await commandService.AddModulesAsync(Assembly.GetExecutingAssembly(),services);
            clientManager.Client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            if (!(s is SocketUserMessage msg))
                return;

            if (msg.Author.IsBot)
                return;

            var argPos = 0;
            if (msg.HasMentionPrefix(clientManager.Client.CurrentUser, ref argPos))
            {
                //ICommandContext context = null;
                //if (clientManager.Client is DiscordSocketClient socketClient)
                //    context = new SocketCommandContext(socketClient, msg);
                //else if (clientManager.Client is DiscordShardedClient shardedClient)
                //    context = new ShardedCommandContext(shardedClient, msg);
                ICommandContext context = new SocketCommandContext(Client, msg);

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

    }
}
