using BoxBot.Core;
using Discord;
using Discord.WebSocket;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BoxBot.Discord
{
    internal class DiscordConnection : IDiscordConnection
    {
        IClientManager clientManager;
        ICommandHandler commandHandler;
        DiscordLogger discordLogger;

        public DiscordConnection(IClientManager clientManager, ICommandHandler commandHandler, DiscordLogger discordLogger)
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
                await commandHandler.Initialize();
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

        private async Task LogRunException(Exception ex) => await discordLogger.Log(new LogMessage(LogSeverity.Critical, typeof(DiscordConnection).GetMethod("RunAsync").Name, "Something went wrong while running the bot. Here is attached an exception", ex));
    }
}
