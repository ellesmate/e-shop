using System.Threading.Tasks;
using Shop.Domain.Models;
using Shop.Domain.Infrastructure;
using System.Linq;

namespace Shop.Application.Cart
{
    [Service]
    public class AddToCart
    {
        private readonly ISessionManager _sessionManager;
        private readonly IStockManager _stockManager;
        private readonly IProductManager _productManager;
        private readonly IProductImageManager _productImageManager;

        public AddToCart(ISessionManager sessionManager, IStockManager stockManager, IProductManager productManager, IProductImageManager productImageManager)
        {
            _sessionManager = sessionManager;
            _stockManager = stockManager;
            _productManager = productManager;
            _productImageManager = productImageManager;
        }

        public class Request
        {
            public int StockId { get; set; }
            public int Qty { get; set; }
        }

        public async Task<bool> Do(Request request)
        {
            if (request.Qty <= 0)
            {
                return false;
            }

            if (! await _stockManager.EnoughStock(request.StockId, request.Qty))
            {
                return false;
            }

            await _stockManager.PutStockOnHold(request.StockId, request.Qty, _sessionManager.GetId());

            var stock = await _stockManager.GetStock(request.StockId);
            var product = await _productManager.GetProductById(stock.ProductId);
            var images = await _productImageManager.GetImages(stock.ProductId);

            var cartProduct = new CartProduct
            {
                ProductId = stock.ProductId,
                ProductName = product.Name,
                StockId = stock.Id,
                Images = images.Select(x => x.Path).ToList(),
                Qty = request.Qty,
                Value = product.Value
            };

            _sessionManager.AddProduct(cartProduct);
            
            return true;
        }
    }
}
