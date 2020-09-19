using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Shop.Database;
using Shop.Application.Products;
using Microsoft.AspNetCore.Http;

namespace Shop.UI.Pages
{
    public class IndexModel : PageModel
    {
        private ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<GetProducts.ProductViewModel> Products { get; set; }

        public void OnGet()
        {
            Products = new GetProducts(_context).Do();
        }

    }
}
