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
        public async Task<IActionResult> GetProducts(string category, [FromServices] GetProducts getProducts, [FromServices] GetProductsByCategory getProductsByCategory) 
        {
            if (!string.IsNullOrWhiteSpace(category))
            {
                return Ok(await getProductsByCategory.Do(category, 0, 100));
            }

            return Ok(await getProducts.Do(0, 100));
        } 


        [HttpGet("{slug}")]
        public async Task<IActionResult> GetProduct(string slug, [FromServices] GetProduct getProduct) => Ok(await getProduct.Do(slug));
    }
}
