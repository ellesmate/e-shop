using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;

namespace Shop.UI.Pages.Support
{
    public class ChatModel : PageModel
    {
        private readonly IChatManager _chatManager;

        public ChatModel(IChatManager chatManager)
        {
            _chatManager = chatManager;

        }
        public Chat Chat { get; set; }
        public async Task OnGet(int id)
        {
            Chat = await _chatManager.GetChatWithMessagesById(id);
        }
    }
}
