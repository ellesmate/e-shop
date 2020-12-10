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
        public IActionResult GetProducts(string category, [FromServices] GetProducts getProducts, [FromServices] GetProductsByCategory getProductsByCategory) 
        {
            if (!string.IsNullOrWhiteSpace(category))
            {
                return Ok(getProductsByCategory.Do(category, 0, 100));
            }

            return Ok(getProducts.Do(0, 100));
        } 


        [HttpGet("{slug}")]
        public async Task<IActionResult> GetProduct(string slug, [FromServices] GetProduct getProduct) => Ok(await getProduct.Do(slug));
    }
}
