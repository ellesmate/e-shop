using Shop.Application.Emails;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Shop.UI.Workers.Email
{
    public class EmailChannel : IEmailSink, IEmailQueue
    {
        private readonly Channel<SendEmailRequest> _channel;

        public EmailChannel()
        {
            _channel = Channel.CreateUnbounded<SendEmailRequest>();
        }

        public ValueTask SendAsync(SendEmailRequest request)
        {
            return _channel.Writer.WriteAsync(request);
        }

        public ValueTask<SendEmailRequest> ReadAsync()
        {
            return _channel.Reader.ReadAsync();
        }
    }
}
