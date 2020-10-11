using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Shop.Database;
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
        private ApplicationDbContext _ctx;
        private IHubContext<ChatHub> _chatHub;

        public ChatController(ApplicationDbContext ctx, IHubContext<ChatHub> chatHub)
        {
            _ctx = ctx;
            _chatHub = chatHub;
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat(string name, string returnUrl = null)
        {
            var chat = new Chat
            {
                Name = User.Identity.Name,
                Users = new List<ChatUser> { 
                    new ChatUser { UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value },
                    new ChatUser { UserId = _ctx.Users.FirstOrDefault(x => x.UserName == "Support").Id }
                } 
            };

            _ctx.Chats.Add(chat);

            await _ctx.SaveChangesAsync();

            if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);

            }
            return Redirect("/Support");
        }

        [HttpPost("[action]/{connectionId}/{roomId}")]
        public async Task<IActionResult> JoinRoom(string connectionId, string roomId)
        {
            // TODO: check is user in this room.
            await _chatHub.Groups.AddToGroupAsync(connectionId, roomId);

            return Ok();
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
            var Message = new Message
            {
                ChatId = roomId,
                Text = message,
                Name = User.Identity.Name,
                Timestamp = DateTime.Now
            };

            _ctx.Add(Message);
            await _ctx.SaveChangesAsync();

            await _chatHub.Clients
                .Group(roomId.ToString())
                .SendAsync("ReceiveMessage", new
                {
                    Message.Text,
                    Message.Name,
                    Timestamp = Message.Timestamp.ToString("dd/MM/yyyy hh:mm:ss")
                });

            return Ok();
        }
    }
}
