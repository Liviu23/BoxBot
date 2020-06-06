using BoxBot.Entities;
using Discord;

namespace BoxBot.Core
{
    public interface IConfiguration
    {
        string DiscordToken { get; set; }
        Entities.ClientType ClientType { get; set; }
        TokenType TokenType { get; set; }
    }
}
