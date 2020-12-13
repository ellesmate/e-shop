using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Orders;

namespace Shop.UI.Pages.Checkout
{
    public class ThankyouModel : PageModel
    {
        public GetOrder.Response Order { get; set; }

        public async Task OnGet([FromServices] GetOrder getOrder, string orderRef)
        {
            Order = await getOrder.Do(orderRef);
        }
    }
}
