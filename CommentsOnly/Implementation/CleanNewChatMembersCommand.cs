using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace CommentsOnly
{
    public class CleanNewChatMembersCommand : IMessagesHandler
    {
        private readonly ITelegramBotClient _client;

        public CleanNewChatMembersCommand(ITelegramBotClient client)
        {
            _client = client;
        }

        public MessageFilter[] Filters { get; } =
            {
                MessageFilters.NewGroupUsers
            };
        
        public async Task HandleAsync(Message message, CancellationToken token)
        {
            User[] newMembers = message.NewChatMembers;
            long chatId = message.Chat.Id;
            
            foreach (User member in newMembers)
            {
                await _client.TemporarilyRemoveUserFromChat(message.Chat, member.Id, token);
            }

            await _client.DeleteMessageAsync(
                chatId,
                message.MessageId,
                token);
        }
    }
}