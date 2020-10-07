using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Application.ProductsAdmin
{
    [Service]
    public class CreateProduct
    {
        private readonly IProductManager _productManager;

        public CreateProduct(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public async Task<Response> Do(Request vm)
        {
            var product = new Product
            {
                Name = vm.Name,
                Description = vm.Description,
                Value = vm.Value,
                Images = vm.Images
            };

            if (await _productManager.CreateProduct(product) <= 0)
            {
                throw new Exception("Failed to create product");
            }

            return new Response
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Value = product.Value
            };
        }
        public class Request
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public List<Image> Images { get; set; } = new List<Image>();
        }
        public class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
        }
    }
}
