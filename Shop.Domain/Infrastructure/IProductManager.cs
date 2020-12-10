using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Domain.Infrastructure
{
    public interface IProductManager
    {
        Task<int> CreateProduct(Product product);
        Task<int> DeleteProduct(int id);
        Task<int> UpdateProduct(Product product);

        TResult GetProductById<TResult>(int id, Func<Product, TResult> selector);
        TResult GetProductBySlug<TResult>(string name, Func<Product, TResult> selector);
        IEnumerable<TResult> GetProducts<TResult>(Func<Product, TResult> selector);
        IEnumerable<TResult> GetProducts<TResult>(Func<Product, TResult> selector, int skip, int take);
        IEnumerable<TResult> GetProductsByCategory<TResult>(string category, Func<Product, TResult> select, int skip, int take);
        public int CountProducts();
    }
}
