using Microsoft.AspNetCore.Mvc;
using Shop.Application.ProductsAdmin;
using Shop.Application.StockAdmin;
using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.UI.Controllers
{
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private ApplicationDbContext _ctx;

        public AdminController(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("products")]
        public IActionResult GetProducts() => Ok(new GetProducts(_ctx).Do());

        [HttpGet("products/{id}")]
        public IActionResult GetProduct(int id) => Ok(new GetProduct(_ctx).Do(id));

        [HttpPost("products")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProduct.Request request) => Ok((await new CreateProduct(_ctx).Do(request)));

        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct(int id) => Ok((await new DeleteProduct(_ctx).Do(id)));

        [HttpPut("products")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProduct.Request request) => Ok((await new UpdateProduct(_ctx).Do(request)));



        [HttpGet("stocks")]
        public IActionResult GetStock() => Ok(new GetStock(_ctx).Do());

        [HttpPost("stocks")]
        public async Task<IActionResult> CreateStock([FromBody] CreateStock.Request request) => Ok((await new CreateStock(_ctx).Do(request)));

        [HttpDelete("stocks/{id}")]
        public async Task<IActionResult> DeleteStock(int id) => Ok((await new DeleteStock(_ctx).Do(id)));

        [HttpPut("stocks")]
        public async Task<IActionResult> UpdateStock([FromBody] UpdateStock.Request request) => Ok((await new UpdateStock(_ctx).Do(request)));
    }
}
