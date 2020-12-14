using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Shop.Application.Chats;
using Shop.UI.Hubs;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shop.UI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ChatController : Controller
    {
        private readonly IHubContext<ChatHub> _chatHub;

        public ChatController(IHubContext<ChatHub> chatHub)
        {
            _chatHub = chatHub;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChat([FromServices] GetChat getChat, int id)
        {
            return Ok(await getChat.Do(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat([FromServices] CreateSupportChat createSupportChat, string returnUrl = null)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                int chatId = await createSupportChat.Do(userId, "Support");
                return Redirect("/Support/Chat/" + chatId);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]/{connectionId}/{roomId}")]
        public async Task<IActionResult> JoinRoom(string connectionId, string roomId, [FromServices] CheckIsUserInChat checkIsUserInChat)
        {
            int chatId = int.Parse(roomId);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (await checkIsUserInChat.Do(chatId, userId))
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
            string message,
            [FromServices] CreateMessage createMessage)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var response = await createMessage.Do(new CreateMessage.Request
                {
                    ChatId = roomId,
                    Text = message,
                    SenderId = userId,
                });

                await _chatHub.Clients
                    .Group(roomId.ToString())
                    .SendAsync("ReceiveMessage", new
                    {
                        response.Text,
                        response.Name,
                        response.Timestamp
                    });
            }
            catch (ArgumentException)
            {
                return Forbid("User doesn't belong to this group.");
            }

            return Ok();
        }
    }
}
