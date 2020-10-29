using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Application.Orders
{
    [Service]
    public class GetOrder
    {
        private readonly IOrderManager _orderManager;

        public GetOrder(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        public class Response
        {
            public string OrderRef { get; set; }

            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }

            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string PostCode { get; set; }

            public IEnumerable<Product> Products { get; set; }
            public string TotalValue { get; set; }
        }

        public class Product
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }
            public int Qty { get; set; }
            public string StockDescription { get; set; }
        }

        public Response Do(string reference) =>
            _orderManager.GetOrderByReference(reference, Projection);

        public static Func<Order, Response> Projection = (order) => 
            new Response
            {
                OrderRef = order.OrderRef,

                FirstName = order.OrderRef,
                LastName = order.OrderRef,
                Email = order.OrderRef,
                PhoneNumber = order.OrderRef,
                Address1 = order.OrderRef,
                Address2 = order.OrderRef,
                City = order.OrderRef,
                PostCode = order.OrderRef,

                Products = order.OrderStocks.Select(y => new Product
                {
                    Name = y.Stock.Product.Name,
                    Description = y.Stock.Product.Description,
                    Value = $"$ {y.Stock.Product.Value:N2}",
                    Qty = y.Qty,
                    StockDescription = y.Stock.Description
                }),

                TotalValue = $"${order.OrderStocks.Sum(y => y.Stock.Product.Value * y.Qty):N2}"
            };
    }
}
