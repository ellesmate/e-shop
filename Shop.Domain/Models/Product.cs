using System.Collections.Generic;

namespace Shop.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Slug { get; set; }
        
        public string Description { get; set; }
        
        public decimal Value { get; set; } // TODO: Rename

        public string Category { get; set; }


        public List<Image> Images { get; set; }
        public ICollection<Stock> Stocks { get; set; }
    }
}
