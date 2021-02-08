using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CommentsOnly
{
    public static class TelegramBotClientExtensions
    {
        public static async Task TemporarilyRemoveUserFromChat(
            this ITelegramBotClient client,
            Chat chat,
            int memberId,
            CancellationToken token)
        {
            var chatId = chat.Id;
            
            await client.KickChatMemberAsync(
                chatId,
                memberId,
                cancellationToken: token);

            if (chat.Type == ChatType.Channel || chat.Type == ChatType.Supergroup)
            {
                await client.UnbanChatMemberAsync(
                    chatId,
                    memberId,
                    token);                
            }
        }
    }
}