using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Database;
using System.Linq;
using System.Security.Claims;

namespace Shop.UI.ViewComponents
{
    public class RoomViewComponent : ViewComponent
    {
        private ApplicationDbContext _ctx;

        public RoomViewComponent(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public IViewComponentResult Invoke()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var chats = _ctx.ChatUsers
                .Include(x => x.Chat)
                    .ThenInclude(x => x.Messages)
                .Where(x => x.UserId == userId)
                .Select(x => x.Chat)
                .ToList();

            return View(chats);
        }
    }
}
