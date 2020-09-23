using Shop.Domain.Models;
using System.Collections.Generic;

namespace Shop.Application.Infrastructure
{
    public interface ISessionManager
    {
        string GetId();
        void AddProduct(int stockId, int qty);
        void RemoveProduct(int stockId, int qty);
        List<CartProduct> GetCart();

        void AddCustomerInformation(CustomerInformation customerInformation);
        CustomerInformation GetCustomerInformation();
    }
}
