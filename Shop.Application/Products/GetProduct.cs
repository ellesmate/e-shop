using Shop.Domain.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Application.Products
{
    [Service]
    public class GetProduct
    {
        private readonly IStockManager _stockManager;
        private readonly IProductManager _productManager;
        private readonly IProductImageManager _productImageManager;

        public GetProduct(IStockManager stockManager, IProductManager productManager, IProductImageManager productImageManager)
        {
            _stockManager = stockManager;
            _productManager = productManager;
            _productImageManager = productImageManager;
        }

        public async Task<ProductViewModel> Do(string slug)
        {
            await _stockManager.RemoveExpiredStockOnHold();

            var product = await _productManager.GetProductWithStocksBySlug(slug);
            var images = await _productImageManager.GetImages(product.Id);

            return new ProductViewModel
            {
                Name = product.Name,
                Description = product.Description,
                Value = product.Value.GetValueString(),
                Slug = product.Slug,
                Images = images.Select(y => y.Path).ToList(),

                Stock = product.Stocks.Select(y => new StockViewModel
                {
                    Id = y.Id,
                    Description = y.Description,
                    Qty = y.Qty
                })
            };
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
