using Discord.WebSocket;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using static BoxBot.Entities.HandlingDelegates;

namespace BoxBot.Core
{
    public interface IDiscordConnection
    {
        Task RunAsync(CancellationToken token = default, DiscordSocketConfig discordSocketConfig = null);
        Task AddModulesAsync(List<Assembly> assemblies);
        Task AddModulesAsync(Assembly assembly);

        void SetOnMessageRecieved(OnMessageRecieved func);
        void SetOnCommandExecuted(OnCommandExecuted func);
        void SetOnExceptionCatched(OnExceptionCatched func);
        void SetTypeReaders(AddTypeReaders func);
    }

}
