using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.Domain.Models;

namespace Shop.Domain.Infrastructure
{
    public interface IStockManager
    {
        Task<int> CreateStock(Stock stock);
        Task<bool> DeleteStock(int id);
        Task<bool> UpdateStockRange(List<Stock> stockList);

        Task<Stock> GetStock(int stockId);
        Task<bool> EnoughStock(int stockId, int qty);
        Task PutStockOnHold(int stockId, int qty, string sessionId);

        Task RemoveStockFromHold(string sessionId);
        Task RemoveStockFromHold(int stockId, int qty, string sessionId);
        Task RemoveExpiredStockOnHold();
    }
}
