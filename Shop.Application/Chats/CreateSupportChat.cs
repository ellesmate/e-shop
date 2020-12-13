using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Chats
{
    public class CreateSupportChat
    {
        private readonly IChatManager _chatManager;

        public CreateSupportChat(IChatManager chatManager)
        {
            _chatManager = chatManager;
        }

        public async Task<int> Do(string memberId, string supportUsername)
        {
            var support = await _chatManager.GetUserByUsername(supportUsername);
            var member = await _chatManager.GetUserWithChatsById(memberId);

            if (member.Chats.Count > 0)
            {
                return member.Chats.First().Id;
            }

            var chatId = await _chatManager.CreateChat(new Chat
            {
                Name = member.Username
            });

            if (chatId <= 0)
            {
                throw new ArgumentException("Failed to create chat.");
            }

            await _chatManager.AddUserToChat(chatId, member.Id);
            await _chatManager.AddUserToChat(chatId, support.Id);

            return chatId;
        }
    }
}
