using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity = Shop.Database.Models;
using Shop.Domain.Models;

namespace Shop.Database.Utils
{
    internal static class Projections
    {
        private static SortedDictionary<object, object> dict = new SortedDictionary<object, object>();

        public static Product EntityProductToDomainProduct(Entity.Product product) =>
            new Product
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                Value = product.Value,
                Category = product.Category,
                //Images = product.Images?.Select(img => EntityImageToDomainImage(img)).ToList(),
                //Stocks = product.Stocks?.Select(s => EntityStockToDomainStock(s)).ToList()
            };

        public static Image EntityImageToDomainImage(Entity.Image image) =>
            new Image
            {
                Id = image.Id,
                Index = image.Index,
                Path = image.Path,
                ProductId = image.ProductId,
            };

        public static Stock EntityStockToDomainStock(Entity.Stock stock) =>
            new Stock
            {
                Id = stock.Id,
                Description = stock.Description,
                Qty = stock.Qty,
                ProductId = stock.Id,
                //Product = EntityProductToDomainProduct(stock.Product)
            };

        public static Order EntityOrderToDomainOrder(Entity.Order order) =>
            new Order
            {
                Id = order.Id,
                OrderRef = order.OrderRef,
                StripeReference = order.StripeReference,
                FirstName = order.FirstName,
                LastName = order.LastName,
                Email = order.Email,
                PhoneNumber = order.PhoneNumber,
                Address1 = order.Address1,
                Address2 = order.Address2,
                City = order.City,
                PostCode = order.PostCode,
                Status = order.Status,
            };

        public static DomainUser EntityUserToDomainUser(Entity.User user) =>
            new DomainUser
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
            };

        public static Chat EntityChatToDomainChat(Entity.Chat chat) =>
            new Chat
            {
                Id = chat.Id,
                Name = chat.Name,
            };

        public static Message EntityMessageToDomainMessage(Entity.Message message) =>
            new Message
            {
                Id = message.Id,
                Text = message.Text,
                Name = message.Name,
                Timestamp = message.Timestamp,
                ChatId = message.ChatId,
            };

        public static Entity.Product DomainProductToEntityProduct(Product product) =>
            new Entity.Product
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                Value = product.Value,
                Category = product.Category,
                //Images = product.Images?.Select(img => DomainImageToEntityImage(img)).ToList(),
                //Stocks = product.Stocks?.Select(s => DomainStockToEntityStock(s)).ToList()
            };

        public static Entity.Image DomainImageToEntityImage(Image image) =>
            new Entity.Image
            {
                Id = image.Id,
                Index = image.Index,
                Path = image.Path,
            };

        public static Entity.Stock DomainStockToEntityStock(Stock stock) =>
            new Entity.Stock
            {
                Id = stock.Id,
                Description = stock.Description,
                Qty = stock.Qty,
                ProductId = stock.Id,
                //Product = DomainProductToEntityProduct(stock.Product)
            };

        public static Entity.Order DomainOrderToEntityOrder(Order order) =>
            new Entity.Order
            {
                Id = order.Id,
                OrderRef = order.OrderRef,
                StripeReference = order.StripeReference,
                FirstName = order.FirstName,
                LastName = order.LastName,
                Email = order.Email,
                PhoneNumber = order.PhoneNumber,
                Address1 = order.Address1,
                Address2 = order.Address2,
                City = order.City,
                PostCode = order.PostCode,
                Status = order.Status,
            };

        public static Entity.Chat DomainChatToEntityChat(Chat chat) =>
            new Entity.Chat
            {
                Id = chat.Id,
                Name = chat.Name,
            };

        public static Entity.Message DomainMessageToEntityMessage(Message message) =>
            new Entity.Message
            {
                Id = message.Id,
                Text = message.Text,
                Name = message.Name,
                Timestamp = message.Timestamp,
                ChatId = message.ChatId,
            };
    }
}
