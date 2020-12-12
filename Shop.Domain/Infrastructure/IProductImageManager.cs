using Shop.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Domain.Infrastructure
{
    public interface IProductImageManager
    {
        Task<IList<Image>> GetImages(int productId);

        Task<bool> AddImage(int productId, Image image);

        Task<bool> RemoveImage(int productId, int imageId);
    }
}
