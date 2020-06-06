using BoxBot.Core;
using Discord;
using System;
using System.Threading.Tasks;

namespace BoxBot.Implementations
{
    public class ConsoleLogger : IDiscordLogger
    {
        public void ColoredLog(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = temp;
        }

        public void LogCritical(string message) => ColoredLog($"Critical: {message}", ConsoleColor.DarkRed);

        public void LogError(string message) => ColoredLog($"Error: {message}", ConsoleColor.Red);

        public void LogWarning(string message) => ColoredLog($"Warning: {message}", ConsoleColor.Yellow);

        public void LogInfo(string message) => ColoredLog($"Info: {message}");

        public void LogVerbose(string message) => ColoredLog($"Verbose: {message}");

        public void LogDebug(string message) => ColoredLog($"Debug: {message}");

        public void Log(string message) => ColoredLog(message);

        public Task Log(LogMessage msg)
        {
            switch (msg.Severity)
            {
                case LogSeverity.Critical:
                    LogCritical(GetStringFromLogMessage(msg));
                    break;
                case LogSeverity.Error:
                    LogError(GetStringFromLogMessage(msg));
                    break;
                case LogSeverity.Warning:
                    LogWarning(GetStringFromLogMessage(msg));
                    break;
                case LogSeverity.Info:
                    LogInfo(GetStringFromLogMessage(msg));
                    break;
                case LogSeverity.Verbose:
                    LogVerbose(GetStringFromLogMessage(msg));
                    break;
                case LogSeverity.Debug:
                    LogDebug(GetStringFromLogMessage(msg));
                    break;
            }
            return Task.CompletedTask;
        }

        private string GetStringFromLogMessage(LogMessage msg) => $"{msg.Message} | From: {msg.Source}{(msg.Exception == null ? "" : $"\n{msg.Exception.Message}")}";

    }
}
