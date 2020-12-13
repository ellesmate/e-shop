using Shop.Domain.Infrastructure;
using System.Threading.Tasks;

namespace Shop.Application.ProductsAdmin
{
    [Service]
    public class GetProduct
    {
        private readonly IProductManager _productManager;

        public GetProduct(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public async Task<ProductViewModel> Do(int id)
        {
            var product = await _productManager.GetProductById(id);

            return new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Value = product.Value,
            };
        }
        public class ProductViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
        }
    }
}
