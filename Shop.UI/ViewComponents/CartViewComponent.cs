﻿using Microsoft.AspNetCore.Mvc;
using Shop.Application.Cart;
using Shop.Domain.Infrastructure;
using System.Linq;

namespace Shop.UI.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private GetCart _getCart;

        public CartViewComponent(GetCart getCart)
        {
            _getCart = getCart;
        }
        public IViewComponentResult Invoke(string view = "Default")
        {
            if (view == "Small")
            {
                var totalValue = _getCart.Do().Sum(x => x.Qty * x.RealValue);
                return View(view, totalValue.GetValueString());
            }

            return View(view, _getCart.Do());
        }
    }
}
