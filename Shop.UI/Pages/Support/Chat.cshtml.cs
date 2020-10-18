using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.UI.Pages.Support
{
    public class ChatModel : PageModel
    {
        private ApplicationDbContext _ctx;

        public ChatModel(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        public Chat Chat { get; set; }
        public void OnGet(int id)
        {
            Chat = _ctx.Chats
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
