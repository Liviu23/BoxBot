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
        IDiscordLogger discordLogger;

        public DiscordConnection(IClientManager clientManager, ICommandHandler commandHandler, IDiscordLogger discordLogger)
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
                clientManager.Client.Log += discordLogger.Log;
                await commandHandler.InitializeAsync();
                await clientManager.Client.StartAsync();
                await Task.Delay(-1, token);
            }
            catch (OperationCanceledException ocex)
            {
                if (!token.IsCancellationRequested || ocex.CancellationToken != token)
                {
                    await LogRunException(ocex);
                    throw ocex;
                }
            }
            catch (Exception ex)
            {
                await LogRunException(ex);
                throw ex;
            }
            finally
            {
                await clientManager.Client?.LogoutAsync();
                clientManager.DisposeOfClient();
            }
        }

        private Task LogRunException(Exception ex) => discordLogger.Log(new LogMessage(LogSeverity.Critical, ex.Source, "Something went wrong while running the bot. Here is attached an exception", ex));
    }
}
