using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Products;

namespace Shop.UI.Pages
{
    public class ShopModel : PageModel
    {
        private static int productsPerPage = 9;

        public IEnumerable<GetProducts.ProductViewModel> Products { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }

        public IActionResult OnGet(
            [FromServices] GetProducts getProducts, 
            [FromServices] CountProducts countProducts,
            int p)
        {
            int count = countProducts.Do();
            int pages = (int) Math.Ceiling((decimal)count / productsPerPage);

            if (p < 1)
            {
                return RedirectToPage("Shop", new { p = 1 });
            }

            if (pages != 0 && p > pages)
            {
                return RedirectToPage("Shop", new { p = pages });
            }

            Products = getProducts.Do(
                (p - 1) * productsPerPage,
                productsPerPage);
            PageNumber = p;
            PageCount = pages;

            return Page();
        }
    }
}
