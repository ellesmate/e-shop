using Microsoft.AspNetCore.Mvc;
using Shop.Application.Cart;
using System.Threading.Tasks;

namespace Shop.UI.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        [HttpPost("add/{stockId}/{qty}")]
        public async Task<IActionResult> AddToCart(
            int stockId,
            int qty,
            [FromServices] AddToCart addToCart)
        {
            var request = new AddToCart.Request
            {
                StockId = stockId,
                Qty = qty
            };

            var success = await addToCart.Do(request);

            if (success)
            {
                return Ok("Item added to cart");
            }
            return BadRequest("Failed to add to cart");
        }

        [HttpPost("remove/{stockId}/{qty}")]
        public async Task<IActionResult> RemoveFromCart(
            int stockId,
            int qty,
            [FromServices] RemoveFromCart removeFromCart)
        {
            var request = new RemoveFromCart.Request
            {
                StockId = stockId,
                Qty = qty
            };

            var success = await removeFromCart.Do(request);

            if (success)
            {
                return Ok("Item removed from cart");
            }
            return BadRequest("Failed to remove item from cart");
        }

        [HttpGet]
        public IActionResult GetCart([FromServices] GetCart getCart) => Ok(getCart.Do());
    }
}
