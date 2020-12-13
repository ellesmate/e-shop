using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Shop.Database.Models
{
    public class User : IdentityUser
    {
        public ICollection<ChatUser> Chats { get; set; }
    }
}
