using Shop.Domain.Infrastructure;

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

        public int Do()
        {
            return _productManager.CountProducts();
        }
    }
}
