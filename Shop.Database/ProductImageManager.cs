using Microsoft.EntityFrameworkCore;
using Shop.Database.Utils;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Database
{
    public class ProductImageManager : IProductImageManager
    {
        private readonly ApplicationDbContext _ctx;

        public ProductImageManager(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IList<Image>> GetImages(int productId)
        {
            var images = await _ctx.Images.Where(x => x.ProductId == productId).ToListAsync();
            return images.Select(Projections.EntityImageToDomainImage).ToList();
        }

        public async Task<bool> AddImage(int productId, Image image)
        {
            var product = await _ctx.Products.FindAsync(productId);
            if (product is null)
            {
                throw new ArgumentException("There is no such product.");
            }

            image.ProductId = productId;

            var entityImage = Projections.DomainImageToEntityImage(image);
            _ctx.Images.Add(entityImage);
            return (await _ctx.SaveChangesAsync()) > 0;
        }

        public async Task<bool> RemoveImage(int productId, int imageId)
        {
            var product = await _ctx.Products.FindAsync(productId);
            if (product is null)
            {
                throw new ArgumentException("There is no such product.");
            }

            var entityImage = await _ctx.Images.FindAsync(imageId);
            if (entityImage is null)
            {
                throw new ArgumentException("There is no such image.");
            }

            _ctx.Images.Remove(entityImage);
            return (await _ctx.SaveChangesAsync()) > 0;
        }
    }
}
