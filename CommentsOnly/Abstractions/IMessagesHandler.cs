using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace CommentsOnly
{
    public interface IMessagesHandler
    {
        MessageFilter[] Filters { get; }
        
        Task HandleAsync(Message message, CancellationToken token);
    }
}