using Telegram.Bot.Types.Enums;

namespace CommentsOnly
{
    public static class MessageFilters
    {
        public static readonly MessageFilter PrivateMessage = msg => msg.Chat.Type == ChatType.Private;
    }
}