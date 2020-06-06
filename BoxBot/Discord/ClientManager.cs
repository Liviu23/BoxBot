using BoxBot.Core;
using Discord.WebSocket;
using Discord;
using System;
using System.Threading.Tasks;

namespace BoxBot.Discord
{
    internal class ClientManager : IClientManager
    {
        /// <summary>
        /// The Discord client
        /// </summary>
        public BaseSocketClient Client { get; private set; }

        public IConfiguration Config { get; private set; }

        public ClientManager(IConfiguration configuration)
        {
            Config = configuration;
        }

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
            await Client.LoginAsync(Config.TokenType, Config.DiscordToken);
        }

        public void DisposeOfClient() => Client = null;
    }
}
