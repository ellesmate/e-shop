
using System.Collections.Generic;

namespace Shop.Domain.Models
{
    public class CartProduct
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal Value { get; set; } // TODO: Rename

        public List<string> Images { get; set; }

        public int StockId { get; set; }

        public int Qty { get; set; }
    }
}
