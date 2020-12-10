using Microsoft.EntityFrameworkCore;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Database
{
    public class ProductManager : IProductManager
    {
        private readonly ApplicationDbContext _ctx;

        public ProductManager(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        private void UpdateSlug(Product product)
        {
            var slug = product.Name.ToLower().Replace(' ', '-');
            product.Slug = slug;
        }

        public Task<int> CreateProduct(Product product)
        {
            UpdateSlug(product);

            _ctx.Products.Add(product);

            return _ctx.SaveChangesAsync();
        }

        public Task<int> DeleteProduct(int id)
        {
            var Product = _ctx.Products.FirstOrDefault(x => x.Id == id);
            _ctx.Products.Remove(Product);
            
            return _ctx.SaveChangesAsync();
        }
        public Task<int> UpdateProduct(Product product)
        {
            UpdateSlug(product);

            _ctx.Products.Update(product);

            return _ctx.SaveChangesAsync();
        }

        public TResult GetProductById<TResult>(int id, Func<Product, TResult> selector)
        {
            return _ctx.Products
                .Where(x => x.Id == id)
                .Select(selector)
                .FirstOrDefault();
        }

        public TResult GetProductBySlug<TResult>(
            string slug, 
            Func<Product, TResult> selector)
        {
            return _ctx.Products
                .Include(x => x.Stock)
                .Include(x => x.Images)
                .Where(x => x.Slug == slug)
                .Select(selector)
                .FirstOrDefault();
        }

        public IEnumerable<TResult> GetProducts<TResult>(Func<Product, TResult> selector) => GetProducts(selector, 0, -1);

        public IEnumerable<TResult> GetProducts<TResult>(Func<Product, TResult> selector, int skip, int take)
        {
            var query = _ctx.Products
                .Skip(skip);

            if (take != -1)
            {
                query = query.Take(take);
            }

            return query
                .Include(x => x.Stock)
                .Include(x => x.Images)
                .Select(selector)
                .ToList();
        }

        public IEnumerable<TResult> GetProductsByCategory<TResult>(string category, Func<Product, TResult> selector, int skip, int take)
        {
            var query = _ctx.Products
                .Where(p => p.Category == category)
                .Skip(skip);

            if (take != -1)
            {
                query = query.Take(take);
            }

            return query
                .Include(x => x.Stock)
                .Include(x => x.Images)
                .Select(selector)
                .ToList();
        }

        public int CountProducts()
        {
            return _ctx.Products.Count();
        }
    }
}
