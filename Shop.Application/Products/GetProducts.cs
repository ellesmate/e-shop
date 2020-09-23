using Microsoft.EntityFrameworkCore;
using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.Application.Products
{
    public class GetProducts
    {
        private ApplicationDbContext _ctx;

        public GetProducts(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<ProductViewModel> Do()
        {
            return _ctx.Products
                .Include(x => x.Stock)
                .Select(x => new ProductViewModel
            {
                Name = x.Name,
                Description = x.Description,
                Value = $"${x.Value:N2}",
                StockCount = x.Stock.Sum(y => y.Qty)
            }).ToList();
        }
        public class ProductViewModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }
            public int StockCount { get; set; }
        }
    }
}
