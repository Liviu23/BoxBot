using BoxBot.Entities;

namespace BoxBot.Core
{
    public interface IConfiguration
    {
        string DiscordToken { get; set; }
        ClientType ClientType { get; set; }
    }
}
