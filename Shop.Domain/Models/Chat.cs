using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Shop.Domain.Models
{
    public class Chat
    {
        public int Id { get; set; }
        
        public string Name { get; set; }


        public ICollection<DomainUser> Users { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
