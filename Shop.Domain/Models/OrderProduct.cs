using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Domain.Models
{
    public class OrderProduct
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int Qty { get; set; }
        public int StockId { get; set; }
        public Stock Stock { get; set; }
    }
}
