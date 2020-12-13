using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Chats
{
    [Service]
    public class CreateMessage
    {
        private readonly IChatManager _chatManager;

        public CreateMessage(IChatManager chatManager)
        {
            _chatManager = chatManager;
        }

        public async Task<Response> Do(Request request)
        {
            if (!await _chatManager.IsUserInChat(request.ChatId, request.SenderId))
            {
                throw new ArgumentException("User doesn't belong to this group.");
            }

            var user = await _chatManager.GetUserById(request.SenderId);

            var message = new Message
            {
                ChatId = request.ChatId,
                Text = request.Text,
                Name = user.Username
            };

            var (messageId, time) = await _chatManager.CreateMessage(message);

            return new Response
            {
                Text = message.Text,
                Name = message.Name,
                Timestamp = time,
            };
        }

        public class Request
        {
            public int ChatId { get; set; }
            public string SenderId { get; set; }
            public string Text { get; set; }
        }

        public class Response
        {
            public string Text { get; set; }
            public string Name { get; set; }
            public DateTime Timestamp { get; set; }
        }

    }
}
