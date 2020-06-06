using Discord;
using System.Threading.Tasks;

namespace BoxBot.Core
{
    public interface IDiscordLogger
    {
        Task Log(LogMessage msg);
    }
}
