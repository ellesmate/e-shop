using Shop.Database;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Products
{
    public class CreateProduct
    {
        private ApplicationDbContext _context;

        public CreateProduct(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Do(string name, string description, decimal value)
        {
            _context.Products.Add(new Product 
            { 
                Name = name, 
                Description = description,
                Value = value
            });

            await _context.SaveChangesAsync();
        }
    }
}
