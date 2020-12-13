using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Chats
{
    public class GetChat
    {
        private IChatManager _chatManager;

        public GetChat(IChatManager chatManager)
        {
            _chatManager = chatManager;
        }

        public Task<Chat> Do(int id) => _chatManager.GetChatWithMessagesById(id);

    }
}
