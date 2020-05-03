using BBox.Discord;
using BoxBot.Core;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using static BoxBot.Entities.HandlingDelegates;

namespace BoxBot.Discord
{
    internal class DiscordConnection : IDiscordConnection
    {
        IClientManager clientManager;
        CommandHandler commandHandler;
        DiscordLogger discordLogger;

        public DiscordConnection(IClientManager clientManager, CommandHandler commandHandler, DiscordLogger discordLogger)
        {
            this.clientManager = clientManager;
            this.commandHandler = commandHandler;
            this.discordLogger = discordLogger;
        }

        public async Task RunAsync(CancellationToken token = default, DiscordSocketConfig discordSocketConfig = null)
        {
            try
            {
                await clientManager.InitializeClientAsync(discordSocketConfig);
                var bClient = clientManager.Client as BaseSocketClient;
                bClient.Log += discordLogger.Log;
                commandHandler.Initialize();
                await bClient.StartAsync();
                await Task.Delay(-1, token);
            }
            catch (OperationCanceledException ocex)
            {
                if (!token.IsCancellationRequested)
                    await LogRunException(ocex);
            }
            catch (Exception ex)
            {
                await LogRunException(ex);
            }
            finally
            {
                var bClient = clientManager.Client as BaseSocketClient;
                if (bClient != null)
                {
                    await bClient.LogoutAsync();
                    clientManager.DisposeOfClient();
                }
            }
        }



        public void SetOnMessageRecieved(OnMessageRecieved func) => commandHandler.handleCommandAsync = func;

        public void SetOnCommandExecuted(OnCommandExecuted func) => commandHandler.commandExecuted = func;

        public void SetOnExceptionCatched(OnExceptionCatched func) => commandHandler.commandServiceLog = func;

        public void SetTypeReaders(AddTypeReaders func) => commandHandler.addTypeReaders = func;



        public async Task AddModulesAsync(List<Assembly> assemblies) => await commandHandler.AddCommandModulesAsync(assemblies);

        public async Task AddModulesAsync(Assembly assembly) => await commandHandler.AddCommandModulesAsync(assembly);



        private async Task LogRunException(Exception ex) => await discordLogger.Log(new LogMessage(LogSeverity.Critical, typeof(DiscordConnection).GetMethod("RunAsync").Name, "Something went wrong while running the bot. Here is attached an exception", ex));
    }
}
