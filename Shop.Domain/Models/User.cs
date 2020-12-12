using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Shop.Domain.Models
{
    public class User
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }
    }
}
