using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Shop.Application.Cart;
using Shop.Database;
using Stripe;

namespace Shop.UI.Pages.Checkout
{
    public class PaymentModel : PageModel
    {
        private ApplicationDbContext _ctx;
        public string PublicKey { get; }

        public PaymentModel(IConfiguration config, ApplicationDbContext ctx)
        {
            _ctx = ctx;
            PublicKey = config["Stripe:PublicKey"].ToString();
        }

        public IActionResult OnGet()
        {
            var information = new GetCustomerInformation(HttpContext.Session).Do();
            if (information == null)
            {
                return RedirectToPage("/Checkout/CustomerInformation");
            }
            
            return Page();
        }

        public IActionResult OnPost(string stripeEmail, string stripeToken)
        {
            var customers = new CustomerService();
            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var CartOrder = new GetOrder(HttpContext.Session, _ctx).Do();

            var options = new ChargeCreateOptions
            {
                Amount = CartOrder.GetTotalCharge(),
                Description = "Shop Purchase",
                Currency = "usd",
                Customer = customer.Id
            };

            var service = new ChargeService();
            var charge = service.Create(options);

            return RedirectToPage("/Index");
        }
    }
}
