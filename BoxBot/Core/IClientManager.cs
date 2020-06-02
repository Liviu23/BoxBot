using Discord.WebSocket;
using System.Threading.Tasks;

namespace BoxBot.Core
{
    public interface IClientManager
    {
        /// <summary>
        /// The Discord client
        /// </summary>
        BaseSocketClient Client { get; }

        /// <summary>
        /// The bot configuration
        /// </summary>
        IConfiguration Config { get; }

        /// <summary>
        /// Creates and logs in a new instance of a client
        /// </summary>
        /// <param name="config">Configuration used by client when logging in</param>
        /// <returns></returns>
        Task InitializeClientAsync(DiscordSocketConfig config = null);

        /// <summary>
        /// Disposes the client
        /// </summary>
        void DisposeOfClient();
    }
}
