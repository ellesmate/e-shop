
namespace Shop.Domain.Models
{
    public class ChatUser
    {
        public string UserId { get; set; }
        public User User { get; set; }


        public int ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}
