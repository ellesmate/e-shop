using Shop.Domain.Enums;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Domain.Infrastructure
{
    public interface IOrderManager
    {
        Task<bool> OrderReferenceExists(string reference);

        Task<IEnumerable<Order>> GetOrdersByStatus(OrderStatus status);
        Task<Order> GetOrderById(int id);
        Task<Order> GetOrderWithWithStocksAndProductsById(int id);

        Task<Order> GetOrderByReference(string reference);
        Task<Order> GetOrderWithWithStocksAndProductsByReference(string reference);


        Task<bool> AdvanceOrder(int id);

        Task<int> CreateOrder(Order order, IEnumerable<OrderStock> stocks);
    }
}
