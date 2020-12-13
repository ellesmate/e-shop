using Microsoft.EntityFrameworkCore;
using Shop.Database.Utils;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityChatUser = Shop.Database.Models.ChatUser;

namespace Shop.Database
{
    public class ChatManager : IChatManager
    {
        private readonly ApplicationDbContext _ctx;

        public ChatManager(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<DomainUser> GetUserById(string id)
        {
            var user = await _ctx.Users.FindAsync(id);
            return Projections.EntityUserToDomainUser(user);
        }

        public async Task<DomainUser> GetUserWithChatsById(string id)
        {
            var user = await _ctx.Users
                .Where(u => u.Id == id)
                .Include(u => u.Chats)
                    .ThenInclude(c => c.Chat)
                .SingleOrDefaultAsync();
            if (user is null)
            {
                throw new ArgumentException("There is no such user.");
            }

            var domainUser = Projections.EntityUserToDomainUser(user);
            domainUser.Chats = user.Chats
                .Select(x => Projections.EntityChatToDomainChat(x.Chat))
                .ToList();

            return domainUser;
        }

        public async Task<DomainUser> GetUserByUsername(string username)
        {
            var user = await _ctx.Users.Where(u => u.UserName == username).SingleOrDefaultAsync();
            if (user is null)
            {
                throw new ArgumentException("There is no such user.");
            }

            return Projections.EntityUserToDomainUser(user);
        }

        public async Task<Chat> GetChatById(int id)
        {
            var chat = await _ctx.Chats.FindAsync(id);
            if (chat is null)
            {
                throw new ArgumentException("There is no such chat.");
            }

            return Projections.EntityChatToDomainChat(chat);
        }

        public async Task<Chat> GetChatWithUsersById(int id)
        {
            var entityChat = await _ctx.Chats
                .Include(c => c.Users)
                    .ThenInclude(u => u.User)
                .Where(c => c.Id == id)
                .SingleOrDefaultAsync();

            if (entityChat is null)
            {
                throw new ArgumentException("There is no such chat.");
            }

            var chat = Projections.EntityChatToDomainChat(entityChat);
            chat.Users = entityChat.Users.Select(u => Projections.EntityUserToDomainUser(u.User)).ToList();
            
            return chat;
        }

        public async Task<Chat> GetChatWithMessagesById(int id)
        {
            var entityChat = await _ctx.Chats.FindAsync(id);
            if (entityChat is null)
            {
                throw new ArgumentException("There is no such chat.");
            }

            await _ctx.Entry(entityChat)
                .Collection(c => c.Messages)
                .LoadAsync();

            var chat = Projections.EntityChatToDomainChat(entityChat);
            chat.Messages = entityChat.Messages.Select(Projections.EntityMessageToDomainMessage).ToList();

            return chat;
        }

        public async Task<int> CreateChat(Chat chat)
        {
            var entityChat = Projections.DomainChatToEntityChat(chat);
            _ctx.Chats.Add(entityChat);

            await _ctx.SaveChangesAsync();

            return entityChat.Id;
        }

        public async Task<bool> AddUserToChat(int chatId, string userId)
        {
            var chat = await _ctx.Chats.FindAsync(chatId);
            if (chat is null)
            {
                throw new ArgumentException("There is no such chat.");
            }

            var user = await _ctx.Users.FindAsync(userId);
            if (user is null)
            {
                throw new ArgumentException("There is no such user.");
            }

            _ctx.ChatUsers.Add(new EntityChatUser
            {
                UserId = user.Id,
                ChatId = chat.Id,
            });

            return (await _ctx.SaveChangesAsync()) > 0;
        }

        public async Task<bool> RemoveUserFromChat(int chatId, string userId)
        {
            var chatUser = await _ctx.ChatUsers.FindAsync(chatId, userId);
            if (chatUser == null)
            {
                throw new ArgumentException("User doesn't belong to this chat.");
            }

            _ctx.ChatUsers.Remove(chatUser);

            return (await _ctx.SaveChangesAsync()) > 0;
        }

        public async Task<int> CreateMessage(int chatId, Message message)
        {
            var entityMessage = Projections.DomainMessageToEntityMessage(message);

            entityMessage.ChatId = chatId;
            entityMessage.Timestamp = DateTime.UtcNow;

            _ctx.Messages.Add(entityMessage);
            await _ctx.SaveChangesAsync();

            return entityMessage.Id;
        }

        public async Task<bool> IsUserInChat(int chatId, string userId)
        {
            var chatUser = await _ctx.ChatUsers.FindAsync(chatId, userId);
            if (chatUser == null)
            {
                return false;
            }

            return true;
        }
    }
}
