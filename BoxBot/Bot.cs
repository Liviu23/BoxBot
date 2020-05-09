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
        public DiscordSocketConfig DiscordSocketConfig { get; set; }
        private CancellationTokenSource Source { get; set; }
        public bool IsRunning { get; private set; }

        public Bot(IConfiguration configuration, IDiscordConnection discordConnection)
        {
            Configuration = configuration;
            DiscordConnection = discordConnection;
        }

        public async Task StartAsync()
        {
            Source = new CancellationTokenSource();
            try
            {
                IsRunning = true;
                await DiscordConnection.RunAsync(Source.Token, DiscordSocketConfig);
            }
            catch (OperationCanceledException ocex)
            {
                if (!Source.IsCancellationRequested)
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
        public async Task Restart()
        {
            Stop();
            await StartAsync();
        }


        public event EventHandler<Exception> ExceptionCatched;
    }
}
