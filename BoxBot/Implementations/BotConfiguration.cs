using BoxBot.Core;
using BoxBot.Entities;
using Discord;
using System;
using System.Diagnostics;

namespace BoxBot.Implementations
{
    public class BotConfiguration : IConfiguration
    {
        public string DiscordToken { get; set; }
        public Entities.ClientType ClientType { get; set; }
        private TokenType tokenType;
        public TokenType TokenType
        {
            get
            {
                return tokenType;
            }
            set
            {
                /* Let only Bots or Users 
                 * I know it is against Discord ToS :\
                 */
                if (value == TokenType.Bot || value == 0)
                    tokenType = value;
                else
                    throw new ArgumentException("Token type must be of type bot or user. User type is against Discord ToS so do not use it");
            }
        }

        public BotConfiguration()
        {
            ClientType = Entities.ClientType.Socket;
            tokenType = TokenType.Bot;
        }
    }
}
