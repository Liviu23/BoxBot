using System.Threading.Tasks;

namespace BoxBot.Core
{
    public interface ICommandHandler
    {
        Task InitializeAsync();
    }
}
