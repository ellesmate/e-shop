using System;
using System.Linq;
using System.Threading.Tasks;
using Shop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Infrastructure;
using System.Collections.Generic;
using EntityStock = Shop.Database.Models.Stock;
using EntityStockOnHold = Shop.Database.Models.StockOnHold; 
using Shop.Database.Utils;

namespace Shop.Database
{
    public class StockManager : IStockManager
    {
        private ApplicationDbContext _ctx;

        public StockManager(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<int> CreateStock(Stock stock)
        {
            var entityStock = Projections.DomainStockToEntityStock(stock);
            _ctx.Stock.Add(entityStock);

            await _ctx.SaveChangesAsync();

            return entityStock.Id;
        }

        public async Task<bool> DeleteStock(int id)
        {
            var stock = await _ctx.Stock.FindAsync(id);
            if (stock is null)
            {
                throw new ArgumentException("There is no such stock.");
            }

            _ctx.Stock.Remove(stock);

            return (await _ctx.SaveChangesAsync()) > 0;
        }

        public async Task<bool> UpdateStockRange(List<Stock> stockList)
        {
            var entityStocks = stockList.Select(s => Projections.DomainStockToEntityStock(s)).ToList();
            _ctx.Stock.UpdateRange(entityStocks);

            return (await _ctx.SaveChangesAsync()) > 0;
        }

        public async Task<bool> EnoughStock(int stockId, int qty)
        {
            var stock = await _ctx.Stock.FindAsync(stockId);
            if (stock is null)
            {
                throw new ArgumentException("There is no such stock.");
            }

            return stock.Qty >= qty;
        }

        public async Task<Stock> GetStock(int stockId)
        {
            var stock = await _ctx.Stock.FindAsync(stockId);
            if (stock is null)
            {
                throw new ArgumentException("There is no such stock.");
            }

            return Projections.EntityStockToDomainStock(stock);
        }

        public async Task PutStockOnHold(int stockId, int qty, string sessionId)
        {
            var entityStock = await _ctx.Stock.FindAsync(stockId);
            if (entityStock is null)
            {
                throw new ArgumentException("There is no such stock.");
            }

            entityStock.Qty -= qty;

            var stockOnHold = await _ctx.StocksOnHold
                .Where(x => x.SessionId == sessionId)
                .ToListAsync();

            if (stockOnHold.Any(x => x.StockId == stockId))
            {
                stockOnHold.Find(x => x.StockId == stockId).Qty += qty;
            }
            else
            {
                _ctx.StocksOnHold.Add(new EntityStockOnHold
                {
                    StockId = stockId,
                    SessionId = sessionId,
                    Qty = qty,
                    ExpiryDate = DateTime.Now.AddMinutes(20)
                });
            }
            
            foreach (var stock in stockOnHold)
            {
                stock.ExpiryDate = DateTime.Now.AddMinutes(20);
            }

            await _ctx.SaveChangesAsync();
        }

        public async Task RemoveStockFromHold(string sessionId)
        {
            var stockOnHold = await _ctx.StocksOnHold
                .Where(x => x.SessionId == sessionId)
                .ToListAsync();

            _ctx.StocksOnHold.RemoveRange(stockOnHold);

            await _ctx.SaveChangesAsync();
        }

        public async Task RemoveStockFromHold(int stockId, int qty, string sessionId)
        {
            var stockOnHold = await _ctx.StocksOnHold
               .SingleOrDefaultAsync(x => x.StockId == stockId && x.SessionId == sessionId);
            if (stockOnHold is null)
            {
                throw new ArgumentException("There is no such stock on hold.");
            }

            var stock = await _ctx.Stock.FindAsync(stockId);
            if (stock is null)
            {
                throw new ArgumentException("There is no such stock.");
            }

            stock.Qty += qty;
            stockOnHold.Qty -= qty;

            if (stockOnHold.Qty <= 0)
            {
                _ctx.Remove(stockOnHold);
            }

            await _ctx.SaveChangesAsync();
        }

        public async Task RemoveExpiredStockOnHold()
        {
            var stocksOnHold = await _ctx.StocksOnHold.Where(x => x.ExpiryDate < DateTime.Now).ToListAsync();

            if (stocksOnHold.Count > 0)
            {
                var stocksId = stocksOnHold.Select(x => x.StockId);
                var stockToReturn = await _ctx.Stock.Where(x => stocksId.Contains(x.Id)).ToListAsync();

                foreach (var stock in stockToReturn)
                {
                    stock.Qty += stocksOnHold.FirstOrDefault(x => x.StockId == stock.Id).Qty;
                }

                _ctx.StocksOnHold.RemoveRange(stocksOnHold);

                await _ctx.SaveChangesAsync();
            }
        }
    }
}
