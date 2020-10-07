using Shop.Domain.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Application.Products
{
    [Service]
    public class GetProducts
    {
        private IProductManager _productManager;

        public GetProducts(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public IEnumerable<ProductViewModel> Do()
        {
            return _productManager
                .GetProducts(x => new ProductViewModel
                {
                    Name = x.Name,
                    Description = x.Description,
                    Value = x.Value.GetValueString(),
                    Slug = x.Slug,
                    StockCount = x.Stock.Sum(y => y.Qty),
                    Images = x.Images.Select(y => y.Path)
                        .Take(2)
                        .ToList()
                });
        }
        public class ProductViewModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }
            public string Slug { get; set; }
            public int StockCount { get; set; }
            public List<string> Images { get; set; }
        }
    }
}
