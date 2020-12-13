using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Chats;
using Shop.Database;
using System.Linq;
using System.Security.Claims;

namespace Shop.UI.ViewComponents
{
    public class RoomViewComponent : ViewComponent
    {
        private readonly GetChats _getChats;

        public RoomViewComponent(GetChats getChats)
        {
            _getChats = getChats;
        }

        public IViewComponentResult Invoke()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var chats = _getChats.Do(userId);

            return View(chats);
        }
    }
}
