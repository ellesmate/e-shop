using Shop.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Chats
{
    [Service]
    public class CheckIsUserInChat
    {
        private readonly IChatManager _chatManager;

        public CheckIsUserInChat(IChatManager chatManager)
        {
            _chatManager = chatManager;
        }

        public Task<bool> Do(int chatId, string userId) => 
            _chatManager.IsUserInChat(chatId, userId);
    }
}
