using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CommentsOnly
{
    public class UpdatesHandler : IUpdatesHandler
    {
        private readonly IEnumerable<IMessagesHandler> _messagesHandlers;

        public UpdatesHandler(
            IEnumerable<IMessagesHandler> messagesHandlers)
        {
            _messagesHandlers = messagesHandlers;
        }

        public Task HandleAsync(Update update, CancellationToken token)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    return HandleMessageAsync(update.Message, token);
                    
                default:
                    return Task.CompletedTask;
            }
        }

        public async Task HandleMessageAsync(Message message, CancellationToken token)
        {
            foreach (IMessagesHandler handler in _messagesHandlers)
            {
                if (handler.Filters.Any(filter => !filter(message)))
                {
                    continue;
                }

                await handler.HandleAsync(message, token);
            }
        }
    }
}