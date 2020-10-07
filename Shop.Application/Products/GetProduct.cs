using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Application.Products
{
    [Service]
    public class GetProduct
    {
        private IStockManager _stockManager;
        private IProductManager _productManager;

        public GetProduct(IStockManager stockManager, IProductManager productManager)
        {
            _stockManager = stockManager;
            _productManager = productManager;
        }

        public async Task<ProductViewModel> Do(string slug)
        {
            await _stockManager.RetrieveExpiredStockOnHold();

            return _productManager
                .GetProductBySlug(slug, x => new ProductViewModel
                {
                    Name = x.Name,
                    Description = x.Description,
                    Value = x.Value.GetValueString(),
                    Slug = x.Slug,
                    Images = x.Images.Select(y => y.Path).ToList(),

                    Stock = x.Stock.Select(y => new StockViewModel
                    {
                        Id = y.Id,
                        Description = y.Description,
                        Qty = y.Qty
                    })
                });
        }
        public class ProductViewModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Slug { get; set; }
            public string Value { get; set; }
            public List<string> Images { get; set; }
            public IEnumerable<StockViewModel> Stock { get; set; }
        }

        public class StockViewModel
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public int Qty { get; set; }
        }
    }
}
