using Microsoft.Extensions.DependencyInjection;
using Shop.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Database
{
    public static class DatabaseManagerRegister
    {
        public static IServiceCollection AddDatabaseManagers(this IServiceCollection @this)
        {
            @this.AddTransient<IStockManager, StockManager>();
            @this.AddTransient<IProductManager, ProductManager>();
            @this.AddTransient<IProductImageManager, ProductImageManager>();
            @this.AddTransient<IOrderManager, OrderManager>();
            @this.AddTransient<IChatManager, ChatManager>();

            return @this;
        }
    }
}
