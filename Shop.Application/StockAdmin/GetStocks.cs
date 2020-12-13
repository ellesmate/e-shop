using Shop.Domain.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Application.StockAdmin
{
    [Service]
    public class GetStocks
    {
        private readonly IProductManager _productManager;

        public GetStocks(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public async Task<IEnumerable<ProductViewModel>> Do()
        {
            var products = await _productManager.GetProductsWithImagesAndStocks(0, 1000);

            return products.Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Stock = x.Stocks.Select(y => new StockViewModel
                {
                    Id = y.Id,
                    Description = y.Description,
                    Qty = y.Qty
                })
            });
        }

        public class StockViewModel
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public int Qty { get; set; }
        }

        public class ProductViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public IEnumerable<StockViewModel> Stock { get; set; }
        }
    }
}
