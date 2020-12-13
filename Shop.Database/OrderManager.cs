using Microsoft.EntityFrameworkCore;
using Shop.Database.Utils;
using Shop.Domain.Enums;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityOrderStock = Shop.Database.Models.OrderStock;

namespace Shop.Database
{
    public class OrderManager : IOrderManager
    {
        private readonly ApplicationDbContext _ctx;
        public OrderManager(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<bool> OrderReferenceExists(string reference)
        {
            return await _ctx.Orders.AnyAsync(x => x.OrderRef == reference);
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatus(OrderStatus status)
        {
            return await _ctx.Orders
                .Where(x => x.Status == status)
                .Select(x => Projections.EntityOrderToDomainOrder(x))
                .ToListAsync();
        }

        //private TResult GetOrder<TResult>(
        //    Expression<Func<Order, bool>> condition,
        //    Func<Order, TResult> selector)
        //{
        //    return _ctx.Orders
        //        .Where(condition)
        //        .Include(x => x.OrderStocks)
        //            .ThenInclude(x => x.Stock)
        //                .ThenInclude(x => x.Product)
        //        .Select(selector)
        //        .FirstOrDefault();
        //}
        public async Task<Order> GetOrderById(int id)
        {
            var order = await _ctx.Orders.FindAsync(id);
            if (order is null)
            {
                throw new ArgumentException("There is no such order.");
            }

            return Projections.EntityOrderToDomainOrder(order);
        }

        public async Task<Order> GetOrderWithWithStocksAndProductsById(int id)
        {
            var entityOrder = await _ctx.Orders
                .Where(x => x.Id == id)
                .Include(x => x.OrderStocks)
                    .ThenInclude(x => x.Stock)
                        .ThenInclude(x => x.Product)
                .SingleOrDefaultAsync();

            if (entityOrder is null)
            {
                throw new ArgumentException("There is no such order.");
            }

            var order = Projections.EntityOrderToDomainOrder(entityOrder);
            order.OrderStocks = entityOrder.OrderStocks.Select(x =>
            {
                var orderStock = new OrderStock
                {
                    StockId = x.StockId,
                    Qty = x.Qty,

                    Stock = Projections.EntityStockToDomainStock(x.Stock),
                };

                orderStock.Stock.Product = Projections.EntityProductToDomainProduct(x.Stock.Product);

                return orderStock;
            });

            return order;
        }

        public async Task<Order> GetOrderByReference(string reference)
        {
            var order = await _ctx.Orders.SingleOrDefaultAsync(x => x.OrderRef == reference);
            if (order is null)
            {
                throw new ArgumentException("There is no such order.");
            }

            return Projections.EntityOrderToDomainOrder(order);
        }

        public async Task<Order> GetOrderWithWithStocksAndProductsByReference(string reference)
        {
            var order = await _ctx.Orders.Where(x => x.OrderRef == reference).SingleOrDefaultAsync();
           
            if (order is null)
            {
                throw new ArgumentException("There is no such order.");
            }

            return await GetOrderWithWithStocksAndProductsById(order.Id);
        }

        public async Task<bool> AdvanceOrder(int id)
        {
            var order = await _ctx.Orders.SingleOrDefaultAsync(x => x.Id == id);
            if (order is null)
            {
                throw new ArgumentException("There is no such order.");
            }

            order.Status++;

            return (await _ctx.SaveChangesAsync()) > 0;
        }

        public async Task<int> CreateOrder(Order order, IEnumerable<OrderStock> orderStocks)
        {
            var entityOrder = Projections.DomainOrderToEntityOrder(order);

            entityOrder.OrderStocks = orderStocks.Select(s => new EntityOrderStock
            {
                StockId = s.StockId,
                Qty = s.Qty,
            }).ToList();
            
            _ctx.Orders.Add(entityOrder);

            await _ctx.SaveChangesAsync();

            return entityOrder.Id;
        }
    }
}
