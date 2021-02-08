using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace CommentsOnly
{
    public interface IUpdatesHandler
    {
        Task HandleAsync(Update update, CancellationToken token);
    }
}