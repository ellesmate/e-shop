using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models
{
    public class CartProduct
    {
        public int StockId { get; set; }
        public int Qty { get; set; }
    }
}
