using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Chats;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;

namespace Shop.UI.Pages.Support
{
    public class ChatModel : PageModel
    {
        private readonly GetChat _getChat;

        public ChatModel(GetChat getChat)
        {
            _getChat = getChat;

        }

        public Chat Chat { get; set; }

        public async Task OnGet(int id)
        {
            Chat = await _getChat.Do(id);
        }
    }
}
