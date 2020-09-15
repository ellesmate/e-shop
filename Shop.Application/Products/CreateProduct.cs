using Shop.Database;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Products
{
    class CreateProduct
    {
        private ApplicationDbContext _context;

        public CreateProduct(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Do(int id, string name, string description)
        {
            _context.Products.Add(new Product 
            { 
                Id = id, 
                Name = name, 
                Description = description 
            });
        }
    }
}
