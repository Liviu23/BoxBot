namespace BoxBot.Core
{
    internal interface IDiscordLogger
    {
        void LogCritical(string message);
        void LogError(string message);
        void LogWarning(string message);
        void LogInfo(string message);
        void LogVerbose(string message);
        void LogDebug(string message);
        void Log(string message);
    }
}
