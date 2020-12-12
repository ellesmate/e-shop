using Shop.Domain.Infrastructure;
using System.Threading.Tasks;

namespace Shop.Application.Products
{
    [Service]
    public class CountProducts
    {
        private IProductManager _productManager;

        public CountProducts(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public async Task<int> Do()
        {
            return await _productManager.CountProducts();
        }
    }
}
