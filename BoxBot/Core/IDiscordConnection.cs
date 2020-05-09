using Discord.WebSocket;
using System.Threading;
using System.Threading.Tasks;

namespace BoxBot.Core
{
    public interface IDiscordConnection
    {
        Task RunAsync(CancellationToken token = default, DiscordSocketConfig discordSocketConfig = null);
    }

}
