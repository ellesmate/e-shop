using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Chats
{
    public class GetChats
    {
        private IChatManager _chatManager;

        public GetChats(IChatManager chatManager)
        {
            _chatManager = chatManager;
        }

        public Task<List<Chat>> Do(string userId) => _chatManager.GetUserChatsWithMessages(userId);
    }
}
