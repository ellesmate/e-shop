using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Shop.Application.Products;
using Shop.Database;

namespace Shop.UI.Pages
{
    public class IndexModel : PageModel
    {
        private ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ProductViewModel Product { get; set; }

        public class ProductViewModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            await new CreateProduct(_context).Do(Product.Name, Product.Description, Product.Value);

            return RedirectToPage("Index");
        }
    }
}
