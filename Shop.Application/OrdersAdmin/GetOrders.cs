using Shop.Domain.Enums;
using Shop.Domain.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Application.OrdersAdmin
{
    [Service]
    public class GetOrders
    {
        private IOrderManager _orderManager;

        public GetOrders(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        public class Response
        {
            public int Id { get; set; }
            public string OrderRef { get; set; }
            public string Email { get; set; }
        }

        public async Task<IEnumerable<Response>> Do(int status)
        {
            var orders = await _orderManager.GetOrdersByStatus((OrderStatus)status);

            return orders.Select(x => new Response
            {
                Id = x.Id,
                OrderRef = x.OrderRef,
                Email = x.Email
            });
        }
    }
}
