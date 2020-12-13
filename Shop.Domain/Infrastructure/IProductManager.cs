using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Domain.Infrastructure
{
    public interface IProductManager
    {
        Task<int> CreateProduct(Product product);
        Task<bool> DeleteProduct(int id);
        Task<bool> UpdateProduct(Product product);

        Task<Product> GetProductById(int id);
        Task<Product> GetProductBySlug(string slug);
        Task<Product> GetProductWithStocksBySlug(string slug);

        Task<IEnumerable<Product>> GetProducts();
        Task<IEnumerable<Product>> GetProducts(int skip, int take);

        Task<IEnumerable<Product>> GetProductsWithImages(int skip, int take);
        Task<IEnumerable<Product>> GetProductsWithImagesAndStocks(int skip, int take);

        Task<IEnumerable<Product>> GetProductsByCategory(string category, int skip, int take);
        Task<IEnumerable<Product>> GetProductsWithImagesByCategory(string category, int skip, int take);
        Task<IEnumerable<Product>> GetProductsWithImagesAndStocksByCategory(string category, int skip, int take);

        public Task<int> CountProducts();
        public Task<int> CountProductsWithCategory(string category);
    }
}
