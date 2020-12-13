using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.OrdersAdmin;
using System.Threading.Tasks;

namespace Shop.UI.Controllers
{
    [Route("[controller]")]
    [Authorize(Policy = ShopConstants.Policies.Manager)]
    public class OrdersController : Controller
    {
        [HttpGet("")]
        public async Task<IActionResult> GetOrders(
            int status, 
            [FromServices] GetOrders getOrders) => Ok(await getOrders.Do(status));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(
            int id,
            [FromServices] GetOrder getOrder) => Ok(await getOrder.Do(id));

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(
            int id,
            [FromServices] UpdateOrder updateOrder) => Ok(await updateOrder.DoAsync(id));
    }
}
