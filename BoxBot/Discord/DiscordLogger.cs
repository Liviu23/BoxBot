using BoxBot.Core;
using Discord;
using System.Threading.Tasks;

namespace BoxBot.Discord
{
    internal class DiscordLogger
    {
        IDiscordLogger logger;

        public DiscordLogger(IDiscordLogger logger)
        {
            this.logger = logger;
        }

        internal Task Log(LogMessage msg)
        {
            switch (msg.Severity)
            {
                case LogSeverity.Critical:
                    logger.LogCritical(msg.Message);
                    break;
                case LogSeverity.Error:
                    logger.LogError(msg.Message);
                    break;
                case LogSeverity.Warning:
                    logger.LogWarning(msg.Message);
                    break;
                case LogSeverity.Info:
                    logger.LogInfo(msg.Message);
                    break;
                case LogSeverity.Verbose:
                    logger.LogVerbose(msg.Message);
                    break;
                case LogSeverity.Debug:
                    logger.LogDebug(msg.Message);
                    break;
            }
            return Task.CompletedTask;
        }
    }
}

