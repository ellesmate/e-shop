using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Shop.Domain.Models
{
    public class DomainUser
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public ICollection<Chat> Chats { get; set; }
    }
}
