using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Database.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public string Category { get; set; }

        public List<Image> Images { get; set; }
        public ICollection<Stock> Stocks { get; set; }
    }
}
