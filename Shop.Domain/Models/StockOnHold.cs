using System;

namespace Shop.Domain.Models
{
    public class StockOnHold
    {
        public int Id { get; set; }
        public string SessionId { get; set; }

        public int StockId { get; set; }

        public int Qty { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
