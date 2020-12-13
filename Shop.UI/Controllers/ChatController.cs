using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using Shop.UI.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shop.UI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ChatController : Controller
    {
        private readonly IHubContext<ChatHub> _chatHub;
        private readonly IChatManager _chatManager;

        public ChatController(ApplicationDbContext ctx, IHubContext<ChatHub> chatHub, IChatManager chatManager)
        {
            _chatHub = chatHub;
            _chatManager = chatManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat(string name, string returnUrl = null)
        {
            var support = await _chatManager.GetUserByUsername("Support");
            var member = await _chatManager.GetUserWithChatsById(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            if (member.Chats.Count > 0)
            {
                return Redirect("/Support/Chat/" + member.Chats.First().Id);
            }

            var chatId = await _chatManager.CreateChat(new Chat
            {
                Name = member.Username
            });

            if (chatId <= 0)
            {
                return BadRequest("Failed to create chat.");
            }
            
            await _chatManager.AddUserToChat(chatId, member.Id);
            await _chatManager.AddUserToChat(chatId, support.Id);

            return Redirect("/Support/Chat/" + chatId);
        }

        [HttpPost("[action]/{connectionId}/{roomId}")]
        public async Task<IActionResult> JoinRoom(string connectionId, string roomId)
        {
            int chatId = int.Parse(roomId);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //var chat = await _chatManager.GetChatWithUsersById(chatId);
            //if (chat.Users.Any(x => x.Id == userId))
            if (await _chatManager.IsUserInChat(chatId, userId))
            {
                await _chatHub.Groups.AddToGroupAsync(connectionId, roomId);
                return Ok();
            }

            return Forbid("User doesn't belong to this group.");
        }

        [HttpPost("[action]/{connectionId}/{roomId}")]
        public async Task<IActionResult> LeaveRoom(string connectionId, string roomId)
        {
            await _chatHub.Groups.RemoveFromGroupAsync(connectionId, roomId);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage(
            int roomId, 
            string message)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!await _chatManager.IsUserInChat(roomId, userId))
            {
                return Forbid("User doesn't belong to this group.");
            }

            var user = await _chatManager.GetUserById(userId);

            var Message = new Message
            {
                ChatId = roomId,
                Text = message,
                Name = user.Username
            };

            var (messageId, time) = await _chatManager.CreateMessage(roomId, Message);

            await _chatHub.Clients
                .Group(roomId.ToString())
                .SendAsync("ReceiveMessage", new
                {
                    Message.Text,
                    Message.Name,
                    time
                });

            return Ok();
        }
    }
}
