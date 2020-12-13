using Shop.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Shop.Domain.Infrastructure
{
    public interface IChatManager
    {
        Task<DomainUser> GetUserById(string id);
        Task<DomainUser> GetUserWithChatsById(string id);
        Task<DomainUser> GetUserByUsername(string username);

        Task<Chat> GetChatById(int id);
        Task<Chat> GetChatWithUsersById(int id);
        Task<Chat> GetChatWithMessagesById(int id);

        Task<int> CreateChat(Chat chat);
        Task<bool> AddUserToChat(int chatId, string userId);
        Task<bool> RemoveUserFromChat(int chatId, string userId);

        Task<(int id, DateTime time)> CreateMessage(int chatId, Message message);

        Task<bool> IsUserInChat(int chatId, string userId);
    }
}
