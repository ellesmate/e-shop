using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shop.Domain.Models;
using System.Threading;

namespace Shop.Database.Tests
{
    public class ProductManagerTests
    {
        [Theory]
        [InlineData("Product name", "product-name")]
        public async Task CreateProductTest(string name, string slug)
        {
            var ctx = new Mock<ApplicationDbContext>();
            ctx.Setup(x => x.Products.Add(It.Is<Product>(x => x.Slug == slug)));
            ctx.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            var result = await new ProductManager(ctx.Object).CreateProduct(new Product
            {
                Name = name,
                Description = "Description",
                Value = 1.1M,
                Images =
                {
                    new Image
                    {
                        Index = 0,
                        Path = "/image1.jpg"
                    },
                    new Image
                    {
                        Index = 1,
                        Path = "/image2.jpg"
                    }
                },
                Stock =
                {
                    new Stock{ },
                    new Stock{ }
                }
            });

            ctx.Verify(x => x.Products.Add(It.Is<Product>(y => y.Slug == slug)), Times.Once);
        }
    }
}
