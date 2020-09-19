using Microsoft.AspNetCore.Mvc;
using Shop.Application.Cart;
using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.UI.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private ApplicationDbContext _ctx;

        public CartViewComponent(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public IViewComponentResult Invoke(string view = "Default")
        {
            return View(view, new GetCart(HttpContext.Session, _ctx).Do());
        }
    }
}
