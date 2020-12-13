using Microsoft.EntityFrameworkCore;
using Shop.Domain.Infrastructure;
using DomainProduct = Shop.Domain.Models.Product;
using EntityProduct = Shop.Database.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Database.Utils;
using Shop.Domain.Models;

namespace Shop.Database
{
    public class ProductManager : IProductManager
    {
        private readonly ApplicationDbContext _ctx;

        public ProductManager(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        private void UpdateSlug(EntityProduct product)
        {
            var slug = product.Name.ToLower().Replace(' ', '-');
            product.Slug = slug;
        }

        public async Task<int> CreateProduct(DomainProduct product)
        {
            var entityProduct = Projections.DomainProductToEntityProduct(product);

            UpdateSlug(entityProduct);

            _ctx.Products.Add(entityProduct);

            await _ctx.SaveChangesAsync();

            return entityProduct.Id;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _ctx.Products.FindAsync(id);
            _ctx.Products.Remove(product);
            
            return (await _ctx.SaveChangesAsync()) > 0;
        }

        public async Task<bool> UpdateProduct(DomainProduct product)
        {
            var entityProduct = Projections.DomainProductToEntityProduct(product);

            UpdateSlug(entityProduct);

            _ctx.Products.Update(entityProduct);

            return (await _ctx.SaveChangesAsync()) > 0;
        }

        public async Task<DomainProduct> GetProductById(int id)
        {
            var entityProduct = await _ctx.Products.FindAsync(id);

            if (entityProduct is null)
            {
                throw new ArgumentException("There is no such product.");
            }

            return Projections.EntityProductToDomainProduct(entityProduct);
        }

        public async Task<DomainProduct> GetProductBySlug(string slug)
        {
            var entityProduct = await _ctx.Products.Where(p => p.Slug == slug).SingleOrDefaultAsync();

            if (entityProduct is null)
            {
                throw new ArgumentException("There is no such product.");
            }

            return Projections.EntityProductToDomainProduct(entityProduct);
        }

        public async Task<Product> GetProductWithStocksBySlug(string slug)
        {
            var entityProduct = await _ctx.Products.Where(p => p.Slug == slug).SingleOrDefaultAsync();

            if (entityProduct is null)
            {
                throw new ArgumentException("There is no such product.");
            }

            await _ctx.Entry(entityProduct)
                .Collection(p => p.Stocks)
                .LoadAsync();

            var product = Projections.EntityProductToDomainProduct(entityProduct);
            product.Stocks = entityProduct.Stocks.Select(Projections.EntityStockToDomainStock).ToList();
            
            return product;
        }


        public Task<IEnumerable<DomainProduct>> GetProducts() => GetProducts(0, 1000);

        public async Task<IEnumerable<DomainProduct>> GetProducts(int skip, int take)
        {
            var query = _ctx.Products
                .Skip(skip)
                .Take(take);

            return await query
                .Select(x => Projections.EntityProductToDomainProduct(x))
                .ToListAsync();
        }
        
        public async Task<IEnumerable<DomainProduct>> GetProductsWithImages(int skip, int take)
        {
            var query = _ctx.Products
                .Include(p => p.Images)
                .Skip(skip)
                .Take(take);

            var products = await query.ToListAsync();
            var domainProducts = products.Select(x =>
            {
                var p = Projections.EntityProductToDomainProduct(x);
                p.Images = x.Images.Select(Projections.EntityImageToDomainImage).ToList();
                return p;
            });

            return domainProducts;
        }

        public async Task<IEnumerable<DomainProduct>> GetProductsWithImagesAndStocks(int skip, int take)
        {
            var query = _ctx.Products
                .Include(p => p.Images)
                .Include(p => p.Stocks)
                .Skip(skip)
                .Take(take);

            var products = await query.ToListAsync();
            var domainProducts = products.Select(x =>
            {
                var p = Projections.EntityProductToDomainProduct(x);
                p.Stocks = x.Stocks.Select(Projections.EntityStockToDomainStock).ToList();
                p.Images = x.Images.Select(Projections.EntityImageToDomainImage).ToList();
                return p;
            });

            return domainProducts;
        }

        public async Task<IEnumerable<DomainProduct>> GetProductsByCategory(string category, int skip, int take)
        {
            var query = _ctx.Products
                .Where(p => p.Category == category)
                .Skip(skip)
                .Take(take);

            return await query
                .Select(x => Projections.EntityProductToDomainProduct(x))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsWithImagesByCategory(string category, int skip, int take)
        {
            var query = _ctx.Products
                .Include(p => p.Images)
                .Where(p => p.Category == category)
                .Skip(skip)
                .Take(take);

            var products = await query.ToListAsync();
            var domainProducts = products.Select(x =>
            {
                var p = Projections.EntityProductToDomainProduct(x);
                p.Images = x.Images.Select(Projections.EntityImageToDomainImage).ToList();
                return p;
            });

            return domainProducts;
        }

        public async Task<IEnumerable<Product>> GetProductsWithImagesAndStocksByCategory(string category, int skip, int take)
        {
            var query = _ctx.Products
                .Include(p => p.Images)
                .Include(p => p.Stocks)
                .Where(p => p.Category == category)
                .Skip(skip)
                .Take(take);

            var products = await query.ToListAsync();
            var domainProducts = products.Select(x =>
            {
                var p = Projections.EntityProductToDomainProduct(x);
                p.Stocks = x.Stocks.Select(Projections.EntityStockToDomainStock).ToList();
                p.Images = x.Images.Select(Projections.EntityImageToDomainImage).ToList();
                return p;
            });

            return domainProducts;
        }

        public Task<int> CountProducts()
        {
            return _ctx.Products.CountAsync();
        }

        public Task<int> CountProductsWithCategory(string category)
        {
            return _ctx.Products
                .Where(p => p.Category == category)
                .CountAsync();
        }
    }
}
