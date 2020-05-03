using Discord.WebSocket;
using Discord;
using System.Threading.Tasks;

namespace BoxBot.Core
{
    internal interface IClientManager
    {
        IDiscordClient Client { get; }
        IConfiguration Config { get; }

        /// <summary>
        /// Creates and logs in a new instance of a client
        /// </summary>
        /// <param name="Token">Bot's login token</param>
        /// <param name="config"></param>
        /// <returns></returns>
        Task InitializeClientAsync(DiscordSocketConfig config = null);

        /// <summary>
        /// Disposes the client
        /// </summary>
        void DisposeOfClient();
    }
}
