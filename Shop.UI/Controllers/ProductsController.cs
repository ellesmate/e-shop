using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.ProductsAdmin;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using Shop.Domain.Models;
using System.Linq;
using Shop.S3;

namespace Shop.UI.Controllers
{
    [Route("[controller]")]
    [Authorize(Policy = ShopConstants.Policies.Manager)]
    public class ProductsController : Controller
    {
        [HttpGet("")]
        public async Task<IActionResult> GetProducts([FromServices] GetProducts getProducts) => Ok(await getProducts.Do());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id, [FromServices] GetProduct getProduct) => Ok(await getProduct.Do(id));

        [HttpPost("")]
        public async Task<IActionResult> CreateProduct(
            [FromForm] TempForm form,
            [FromServices] CreateProduct createProduct,
            [FromServices] S3Client s3Client)
        {
            var product = new CreateProduct.Request
            {
                Name = form.Name,
                Description = form.Description,
                Value = form.Value
            };

            var results = await Task.WhenAll(UploadFiles(s3Client, form.Images));

            product.Images.AddRange(results.Select((path, index) => new Image
            {
                Index = index,
                Path = path,
            }));

            return Ok(await createProduct.Do(product));
        }

        private static IEnumerable<Task<string>> UploadFiles(S3Client s3Client, IEnumerable<IFormFile> files)
        {
            var index = 0;
            foreach (var image in files)
            {
                var fileName = $"{DateTime.Now.Ticks}_{index++}{Path.GetExtension(image.FileName)}";
                yield return s3Client.SavePublicFile($"images/{fileName}", image.OpenReadStream());
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
            [FromServices] UpdateProduct updateProduct) => Ok(await updateProduct.Do(request));
    }
}
