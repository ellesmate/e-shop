using System;

namespace Shop.Domain.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string Name { get; set; }

        public DateTime Timestamp { get; set; }

        public int ChatId { get; set; }
    }
}
