using System.Linq;
using Telegram.Bot.Types.Enums;

namespace CommentsOnly
{
    public static class MessageFilters
    {
        public static readonly MessageFilter PrivateMessage = msg => msg.Chat.Type == ChatType.Private;
        
        public static readonly MessageFilter NewGroupUsers = msg => msg.Chat.Type == ChatType.Group && msg.NewChatMembers?.Any() == true;
    }
}