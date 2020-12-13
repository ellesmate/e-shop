using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop.UI.Infrastructure
{
    public class SessionManager : ISessionManager
    {
        private const string KeyCart = "cart";
        private const string KeyCustomerInfo = "customer-info";

        private readonly ISession _session;

        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }
        public string GetId() => _session.Id;

        public void AddProduct(CartProduct cartProduct)
        {

            var cartList = new List<CartProduct>();
            var stringObject = _session.GetString(KeyCart);

            if (!string.IsNullOrEmpty(stringObject))
            {
                cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);
            }

            if (cartList.Any(x => x.StockId == cartProduct.StockId))
            {
                cartList.Find(x => x.StockId == cartProduct.StockId).Qty += cartProduct.Qty;
            }
            else
            {
                cartList.Add(cartProduct);
            }

            stringObject = JsonConvert.SerializeObject(cartList);
            _session.SetString(KeyCart, stringObject);
        }

        public void RemoveProduct(int stockId, int qty)
        {
            var stringObject = _session.GetString(KeyCart);

            if (string.IsNullOrEmpty(stringObject))
                return;
            
            List<CartProduct> cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);

            if (!cartList.Any(x => x.StockId == stockId))
                return;

            var product = cartList.First(x => x.StockId == stockId);
            product.Qty -= qty;

            if (product.Qty <= 0)
            {
                cartList.Remove(product);
            }

            stringObject = JsonConvert.SerializeObject(cartList);
            _session.SetString(KeyCart, stringObject);
        }

        public IEnumerable<TResult> GetCart<TResult>(Func<CartProduct, TResult> selector)
        {
            var stringObject = _session.GetString(KeyCart);

            if (string.IsNullOrEmpty(stringObject))
                return new List<TResult>();

            return JsonConvert.DeserializeObject<IEnumerable<CartProduct>>(stringObject)
                .Select(selector);
        }
        
        public void ClearCart()
        {
            _session.Remove(KeyCart);
        }

        public void AddCustomerInformation(CustomerInformation customerInformation)
        {
            var stringObject = JsonConvert.SerializeObject(customerInformation);

            _session.SetString(KeyCustomerInfo, stringObject);
        }

        public CustomerInformation GetCustomerInformation()
        {
            var stringObject = _session.GetString(KeyCustomerInfo);

            if (string.IsNullOrEmpty(stringObject))
                return null;

            return JsonConvert.DeserializeObject<CustomerInformation>(stringObject);
        }
    }
}
