using BoxBot.Core;
using System;

namespace BoxBot.Implementations
{
    internal class ConsoleLogger : IDiscordLogger
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
    }
}
