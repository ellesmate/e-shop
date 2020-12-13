using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using Shop.Application.Cart;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Shop.Application.Tests
{
    public class CartTests
    {

        [Fact]
        public void GetCustomerInformationTest()
        {
            var customerInformation = new CustomerInformation
            {
                FirstName = "Bob",
                LastName = "Smith",
                Email = "bobsmith@gmail.com",
                PhoneNumber = "375123092723",
                Address1 = "Lenina street 12",
                Address2 = "",
                City = "Minsk",
                PostCode = "220000"
            };

            var mock = new Mock<ISessionManager>();
            mock.Setup(x => x.GetCustomerInformation()).Returns(customerInformation);

            GetCustomerInformation.Response response = 
                new GetCustomerInformation(mock.Object).Do();

            Assert.True(
                response.FirstName == customerInformation.FirstName &&
                response.LastName == customerInformation.LastName &&
                response.Email == customerInformation.Email &&
                response.PhoneNumber == customerInformation.PhoneNumber &&
                response.Address1 == customerInformation.Address1 &&
                response.Address2 == customerInformation.Address2 &&
                response.City == customerInformation.City &&
                response.PostCode == customerInformation.PostCode
            );
        }

        [Fact]
        public void AddCustomerInformationTest()
        {
            var request = new AddCustomerInformation.Request
            {
                FirstName = "Bob",
                LastName = "Smith",
                Email = "bobsmith@gmail.com",
                PhoneNumber = "375123092723",
                Address1 = "Lenina street 12",
                Address2 = "",
                City = "Minsk",
                PostCode = "220000"
            };

            var mock = new Mock<ISessionManager>();
            new AddCustomerInformation(mock.Object).Do(request);
            
            mock.Verify(x => x.AddCustomerInformation(It.Is<CustomerInformation>(y =>
                y.FirstName == request.FirstName &&
                y.LastName == request.LastName &&
                y.Email == request.Email &&
                y.PhoneNumber == request.PhoneNumber &&
                y.Address1 == request.Address1 &&
                y.Address2 == request.Address2 &&
                y.City == request.City &&
                y.PostCode == request.PostCode
            )), Times.Once);
        }

        [Fact]
        public void GetCartTest()
        {
            var products = new List<CartProduct> {
                new CartProduct
                {
                    ProductId = 1,
                    ProductName = "Product 1",
                    Value = 0.25M,
                    StockId = 1,
                    Qty = 1
                },
                new CartProduct
                {
                    ProductId = 2,
                    ProductName = "Product 2",
                    Value = 10.99M,
                    StockId = 2,
                    Qty = 3
                }
            };

            var mock = new Mock<ISessionManager>();

            mock.Setup(p => p.GetCart(It.IsAny<Func<CartProduct, GetCart.Response>>()))
                .Returns<Func<CartProduct, GetCart.Response>>(selector => products.Select(selector).ToList());

            var cart = new GetCart(mock.Object).Do().ToList();

            var ans = new List<GetCart.Response>()
            {
                new GetCart.Response
                {
                    Name = "Product 1",
                    RealValue = 0.25M,
                    Value = "$0.25",
                    Qty = 1,
                    StockId = 1
                },
                new GetCart.Response
                {
                    Name = "Product 2",
                    RealValue = 10.99M,
                    Value = "$10.99",
                    Qty = 3,
                    StockId = 2
                }
            };

            Assert.Equal(ans.Count(), cart.Count());
            for (int i = 0; i < cart.Count(); i++)
            {
                var product = ans.FirstOrDefault(x =>
                    x.Name == cart[i].Name &&
                    x.RealValue == cart[i].RealValue &&
                    x.Value == cart[i].Value &&
                    x.Qty == cart[i].Qty &&
                    x.StockId == cart[i].StockId
                );

                Assert.NotNull(product);
                ans.Remove(product);
            }
        }
    
        [Theory]
        [InlineData(1, 2, 3, true)]
        [InlineData(1, 100, 100000, true)]
        [InlineData(2, 4, 3, false)]
        [InlineData(3, -1, 3, false)]
        [InlineData(4, -10, 0, false)]
        public async Task AddToCartTest(int stockId, int qty, int allStocks, bool expectedResult)
        {
            //var stock = new Stock
            //{
            //    Id = stockId,
            //    Description = "Stock description",
            //    ProductId = 2,
            //    Product = new Product 
            //    {
            //        Id = 2,
            //        Name = "Product name",
            //        Description = "Product description",
            //        Value = 1.5M
            //    },
            //    Qty = allStocks
            //};
            //var request = new AddToCart.Request
            //{
            //    StockId = stockId,
            //    Qty = qty
            //};

            //var sessionMock = new Mock<ISessionManager>();
            //sessionMock.Setup(x => x.GetId()).Returns("");

            //var stockMock = new Mock<IStockManager>();

            //stockMock
            //    .Setup(x => x.EnoughStock(It.IsAny<int>(), It.IsAny<int>()))
            //    .Returns<int, int>((stockId, qty) => Task.FromResult(qty <= allStocks));

            //stockMock
            //    .Setup(x => x.GetStock(It.IsAny<int>()))
            //    .ReturnsAsync(stock);

            //var result = await new AddToCart(sessionMock.Object, stockMock.Object).Do(request);
            
            //Assert.Equal(expectedResult, result);

            //var times = result ? Times.Once() : Times.Never();

            //sessionMock.Verify(x => x.AddProduct(It.IsNotNull<CartProduct>()), times);
            //sessionMock.Verify(x => x.GetId(), times);
            
            //stockMock.Verify(x => x.PutStockOnHold(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), times);
        }
    }
}
