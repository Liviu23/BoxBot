using BoxBot.Core;
using BoxBot.Entities;

namespace BoxBot.Implementations
{
    public class BotConfiguration : IConfiguration
    {
        public string DiscordToken { get; set; }
        public ClientType ClientType { get; set; }
    }
}
