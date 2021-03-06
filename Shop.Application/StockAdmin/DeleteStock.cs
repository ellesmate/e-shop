﻿using Shop.Domain.Infrastructure;
using System.Threading.Tasks;

namespace Shop.Application.StockAdmin
{
    [Service]
    public class DeleteStock
    {
        private readonly IStockManager _stockManager;

        public DeleteStock(IStockManager stockManager)
        {
            _stockManager = stockManager;
        }

        public Task<bool> Do(int id)
        {
            return _stockManager.DeleteStock(id);
        }
    }
}
