using BoxBot.Core;
using Discord;
using System.Text;
using System.Threading.Tasks;

namespace BoxBot.Discord
{
    internal class DiscordLogger
    {
        IDiscordLogger logger;
        StringBuilder builder;

        public DiscordLogger(IDiscordLogger logger)
        {
            this.logger = logger;
            builder = new StringBuilder();
        }

        internal Task Log(LogMessage msg)
        {
            switch (msg.Severity)
            {
                case LogSeverity.Critical:
                    logger.LogCritical(GetStringFromLogMessage(msg));
                    break;
                case LogSeverity.Error:
                    logger.LogError(GetStringFromLogMessage(msg));
                    break;
                case LogSeverity.Warning:
                    logger.LogWarning(GetStringFromLogMessage(msg));
                    break;
                case LogSeverity.Info:
                    logger.LogInfo(GetStringFromLogMessage(msg));
                    break;
                case LogSeverity.Verbose:
                    logger.LogVerbose(GetStringFromLogMessage(msg));
                    break;
                case LogSeverity.Debug:
                    logger.LogDebug(GetStringFromLogMessage(msg));
                    break;
            }
            return Task.CompletedTask;
        }

        private string GetStringFromLogMessage(LogMessage msg) 
        {
            builder.Clear();
            builder.Append($"{msg.Message} | From: {msg.Source}");
            if (msg.Exception != null)
                builder.Append($"\n{msg.Exception.Message}");
            return builder.ToString();
        }
    }
}


