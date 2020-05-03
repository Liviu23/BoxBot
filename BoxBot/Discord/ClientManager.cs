using BoxBot.Core;
using Discord.WebSocket;
using Discord;
using System;
using System.Threading.Tasks;

namespace BoxBot.Discord
{
    internal class ClientManager : IClientManager
    {
        public IDiscordClient Client { get; private set; }
        public IConfiguration Config { get; private set; }

        public ClientManager(IConfiguration configuration)
        {
            Config = configuration;
        }

        /// <summary>
        /// Creates and logs in a new instance of a client
        /// </summary>
        /// <param name="Token">Bot's login token</param>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task InitializeClientAsync(DiscordSocketConfig config = null)
        {
            if (config == null)
                config = new DiscordSocketConfig() { LogLevel = LogSeverity.Verbose };
            switch (Config.ClientType)
            {
                case Entities.ClientType.Sharded:
                    Client = new DiscordShardedClient(config);
                    break;
                case Entities.ClientType.Socket:
                    Client = new DiscordSocketClient(config);
                    break;
                default:
                    throw new Exception("Configuration does not contain a recognized client type");
            }
            await (Client as BaseSocketClient).LoginAsync(TokenType.Bot, Config.DiscordToken);
        }

        /// <summary>
        /// Disposes the client
        /// </summary>
        public void DisposeOfClient() => Client = null;
    }
}
