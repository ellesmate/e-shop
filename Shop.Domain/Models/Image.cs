using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models
{
    public class Image
    {
        public int Id { get; set; }

        public int Index { get; set; }

        public string Path { get; set; }

        public int ProductId { get; set; }
    }
}
