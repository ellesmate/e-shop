using Microsoft.AspNetCore.Mvc;
using Shop.Application.Cart;
using Shop.Application.Orders;
using Shop.Domain.Infrastructure;
using Stripe;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.UI.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutController : ControllerBase
    {
        [HttpPost("addCustomerInfo")]
        public IActionResult AddCustomerInformation(
            AddCustomerInformation.Request customerInformation,
            [FromServices] AddCustomerInformation addCustomerInformation)
        {
            if (ModelState.IsValid)
            {
                addCustomerInformation.Do(customerInformation);
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("pay")]
        public async Task<IActionResult> Pay(
           string stripeEmail,
           string stripeToken,
           [FromServices] Application.Cart.GetOrder getOrder,
           [FromServices] CreateOrder createOrder,
           [FromServices] ISessionManager sessionManager)
        {
            var customers = new CustomerService();
            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var cartOrder = getOrder.Do();

            var options = new ChargeCreateOptions
            {
                Amount = cartOrder.GetTotalCharge(),
                Description = "Shop Purchase",
                Currency = "usd",
                Customer = customer.Id
            };

            var service = new ChargeService();
            var charge = service.Create(options);

            var sessionId = HttpContext.Session.Id;

            await createOrder.Do(new CreateOrder.Request
            {
                StripeReference = charge.Id,
                SessionId = sessionId,

                FirstName = cartOrder.CustomerInformation.FirstName,
                LastName = cartOrder.CustomerInformation.LastName,
                Email = cartOrder.CustomerInformation.Email,
                PhoneNumber = cartOrder.CustomerInformation.PhoneNumber,
                Address1 = cartOrder.CustomerInformation.Address1,
                Address2 = cartOrder.CustomerInformation.Address2,
                City = cartOrder.CustomerInformation.City,
                PostCode = cartOrder.CustomerInformation.PostCode,

                Stocks = cartOrder.Products.Select(x => new CreateOrder.Stock
                {
                    StockId = x.StockId,
                    Qty = x.Qty
                }).ToList()
            });

            sessionManager.ClearCart();

            return Ok();
        }

        ///////////////////////////////////////////////////////////////////
        //[HttpPost("key/{apiVersion}")]
        //public IActionResult GenerateEphemeralKey(string apiVersion)
        //{
        //    var customers = new CustomerService();

        //    var customer = customers.Create(new CustomerCreateOptions
        //    {
        //        Email = "alsdkf@adasdf.com"
        //    });

        //    var options = new EphemeralKeyCreateOptions
        //    {
        //        Customer = customer.Id,
        //        StripeVersion = apiVersion
        //    };
        //    var service = new EphemeralKeyService();
        //    var key = service.Create(options);

        //    return Ok(key.ToJson());
        //}

        //[HttpPost("payment")]
        //public IActionResult Payment(
        //    [FromServices] Application.Cart.GetOrder getOrder,
        //    [FromServices] CreateOrder createOrder,
        //    [FromServices] ISessionManager sessionManager)
        //{
        //    var options = new PaymentIntentCreateOptions
        //    {
        //        Amount = 1100,
        //        Description = "Shop Purchase",
        //        Currency = "usd",
        //    };

        //    var service = new PaymentIntentService();
        //    var paymentIntent = service.Create(options);
        //    //return Ok(paymentIntent.ToJson());
        //    //return Ok(paymentIntent.PaymentMethodId);
        //    return Ok(paymentIntent.ClientSecret);
        //}

    }
}
