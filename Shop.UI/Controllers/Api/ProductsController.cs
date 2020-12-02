using Microsoft.AspNetCore.Mvc;
using Shop.Application.Products;
using System.Threading.Tasks;

namespace Shop.UI.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult GetProducts([FromServices] GetProducts getProducts) => Ok(getProducts.Do(0, 10));

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetProduct(string slug, [FromServices] GetProduct getProduct) => Ok(await getProduct.Do(slug));
    }
}
