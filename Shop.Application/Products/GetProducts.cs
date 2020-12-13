using Shop.Domain.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<ProductViewModel>> Do(int skip, int take)
        {
            var products = await _productManager.GetProductsWithImagesAndStocks(skip, take);

            return products.Select(x => new ProductViewModel
            {
                Name = x.Name,
                Description = x.Description,
                Value = x.Value.GetValueString(),
                Slug = x.Slug,
                StockCount = x.Stocks.Sum(y => y.Qty),
                Images = x.Images.Select(y => y.Path)
                        .Take(2)
                        .ToList()
            }).ToList();
        }

        public async Task<IEnumerable<ProductViewModel>> Do(string category, int skip, int take)
        {
            var products = await _productManager.GetProductsWithImagesAndStocksByCategory(category, skip, take);

            return products.Select(x => new ProductViewModel
            {
                Name = x.Name,
                Description = x.Description,
                Value = x.Value.GetValueString(),
                Slug = x.Slug,
                StockCount = x.Stocks.Sum(y => y.Qty),
                Images = x.Images.Select(y => y.Path)
                        .Take(2)
                        .ToList()
            }).ToList();
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
