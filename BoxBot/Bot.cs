using BoxBot.Core;
using Discord.WebSocket;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BoxBot
{
    public class Bot
    {
        public IConfiguration Configuration { get; set; }
        private IDiscordConnection DiscordConnection { get; set; }
        /// <summary>
        /// Client will use this configuration when logging in
        /// </summary>
        public DiscordSocketConfig DiscordSocketConfig { get; set; }
        private CancellationTokenSource Source { get; set; }
        /// <summary>
        /// Returns whether the bot is running or not
        /// </summary>
        public bool IsRunning { get; private set; }

        public Bot(IConfiguration configuration, IDiscordConnection discordConnection)
        {
            Configuration = configuration;
            DiscordConnection = discordConnection;
        }

        public async Task StartAsync()
        {
            if (IsRunning)
                return;

            IsRunning = true;
            Source = new CancellationTokenSource();
            try
            {
                await DiscordConnection.RunAsync(Source.Token, DiscordSocketConfig);
            }
            catch (OperationCanceledException ocex)
            {
                if (!Source.IsCancellationRequested || ocex.CancellationToken != Source.Token)
                    ExceptionCatched?.Invoke(this, ocex);
            }
            catch (Exception ex)
            {
                ExceptionCatched?.Invoke(this, ex);
            }
            finally
            {
                IsRunning = false;
            }
        }
        public void Stop()
        {
            if (Source != null)
                Source.Cancel();
        }
        public void Stop(int delay)
        {
            if (Source != null)
                Source.CancelAfter(delay);
        }
        public void Stop(TimeSpan delay)
        {
            if (Source != null)
                Source.CancelAfter(delay);
        }
        public async Task RestartAsync()
        {
            Stop();
            await StartAsync();
        }


        public event EventHandler<Exception> ExceptionCatched;
    }
}
