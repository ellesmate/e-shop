using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.ProductsAdmin;
using Shop.Database;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Shop.Domain.Models;
using System.Linq;

namespace Shop.UI.Controllers
{
    [Route("[controller]")]
    [Authorize(Policy = "Manager")]
    public class ProductsController : Controller
    {
        private ApplicationDbContext _ctx;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ApplicationDbContext ctx, IWebHostEnvironment webHostEnvironment)
        {
            _ctx = ctx;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("")]
        public IActionResult GetProducts([FromServices] GetProducts getProducts) => Ok(getProducts.Do());

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id, [FromServices] GetProduct getProduct) => Ok(getProduct.Do(id));

        [HttpPost("")]
        public async Task<IActionResult> CreateProduct(
            [FromForm] TempForm form,
            [FromServices] CreateProduct createProduct)
        {

            var product = new CreateProduct.Request
            {
                Name = form.Name,
                Description = form.Description,
                Value = form.Value
            };

            var results = await Task.WhenAll(UploadFiles());

            product.Images.AddRange(results.Select((path, index) => new Image
            {
                Index = index,
                Path = path,
            }));

            return Ok((await createProduct.Do(product)));

            IEnumerable<Task<string>> UploadFiles()
            {
                var index = 0;
                
                foreach (var image in form.Images)
                {
                    var fileName = $"{DateTime.Now.Ticks}_{index++}{Path.GetExtension(image.FileName)}";

                    yield return SaveFile(fileName, image.OpenReadStream());
                }
            }

            async Task<string> SaveFile(string fileName, Stream fileStream)
            {
                var folder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                var filePath = Path.Combine(folder, fileName);

                using (var newStream = new FileStream(filePath, FileMode.Create))
                {
                    await fileStream.CopyToAsync(newStream);
                    return filePath.Replace(_webHostEnvironment.WebRootPath, "").Replace(@"\", "/");
                }
            }
        }

        public class TempForm
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public IEnumerable<IFormFile> Images { get; set; }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(
            int id,
            [FromServices] DeleteProduct deleteProduct) => Ok(await deleteProduct.Do(id));

        [HttpPut("")]
        public async Task<IActionResult> UpdateProduct(
            [FromBody] UpdateProduct.Request request,
            [FromServices] UpdateProduct updateProduct) => Ok((await updateProduct.Do(request)));
    }
}
