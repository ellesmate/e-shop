using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Shop.Application.ProductsAdmin;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using Xunit;

namespace Shop.Application.Tests
{
    public class ProductAdminTests
    {
        [Theory]
        [InlineData("First Product", "Default description", 2.2, 2, false)]
        [InlineData("Second Product", "Default description2", 199.2, 0, true)]
        [InlineData("Third Product", "Third description", 129.2, 199, false)]
        [InlineData("Third Product", "Third description", 129.2, 0, true)]
        public async Task CreateProductTest(string name, string description, decimal value, int saved, bool error)
        {
            var mock = new Mock<IProductManager>();
            mock.Setup(x => x.CreateProduct(It.IsAny<Product>()))
                .Returns(Task.FromResult(saved));

            var vm = new CreateProduct.Request
            {
                Name = name,
                Description = description,
                Value = value,
                Images =
                {
                    new Image { Index = 0, Path = "/image1"},
                    new Image { Index = 1, Path = "/image2"},
                    new Image { Index = 2, Path = "/image3"}
                }
            };

            Func<Task<CreateProduct.Response>> act = () => new CreateProduct(mock.Object).Do(vm);

            if (error)
            {
                var exception = await Assert.ThrowsAsync<Exception>(act);

                Assert.Equal("Failed to create product", exception.Message);
                mock.Verify(x => x.CreateProduct(It.IsAny<Product>()), Times.Once);
            }
            else
            {
                CreateProduct.Response response = await act();
                mock.Verify(x => x.CreateProduct(It.IsAny<Product>()), Times.Once);
                Assert.Equal(name, response.Name);
                Assert.Equal(description, response.Description);
                Assert.Equal(value, response.Value);
            }
        }

        [Theory]
        [InlineData(1, "New product name", "New product description", 1999)]
        [InlineData(2, "New second name", "New second description", 1.99)]
        [InlineData(3, "New-product-name", "New product description", 20)]
        public async Task UpdateProductTest(int id, string name, string description, decimal value)
        {
            var request = new UpdateProduct.Request
            {
                Id = id,
                Name = name,
                Description = description,
                Value = value
            };

            var mock = new Mock<IProductManager>();
            mock.Setup(x => x.UpdateProduct(It.Is<Product>(y => y.Id > 0)))
                .Returns(Task.FromResult(1));

            var response = await new UpdateProduct(mock.Object).Do(request);

            Assert.Equal(id, response.Id);
            Assert.Equal(name, response.Name);
            Assert.Equal(description, response.Description);
            Assert.Equal(value, response.Value);
        }
    }
}
