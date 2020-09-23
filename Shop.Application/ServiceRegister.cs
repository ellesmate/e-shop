using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Cart;
using Shop.Application.OrdersAdmin;
using Shop.Application.UsersAdmin;

namespace Shop.Application
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection @this)
        {
            @this.AddTransient<AddCustomerInformation>();
            @this.AddTransient<AddToCart>();
            @this.AddTransient<GetCart>();
            @this.AddTransient<GetCustomerInformation>();
            @this.AddTransient<Cart.GetOrder>();
            @this.AddTransient<RemoveFromCart>();

            @this.AddTransient<OrdersAdmin.GetOrder>();
            @this.AddTransient<GetOrders>();
            @this.AddTransient<UpdateOrder>();

            @this.AddTransient<CreateUser>();

            return @this;
        }
    }
}
