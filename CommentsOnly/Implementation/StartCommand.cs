using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CommentsOnly
{
    public class StartCommand : IMessagesHandler
    {
        private readonly ITelegramBotClient _client;

        public StartCommand(ITelegramBotClient client)
        {
            _client = client;
        }

        public MessageFilter[] Filters { get; } =
            {
                MessageFilters.PrivateMessage
            };

        public async Task HandleAsync(Message message, CancellationToken token)
        {
            await _client.SendTextMessageAsync(
                message.Chat,
                "Hey!",
                cancellationToken: token);
        }
    }
}