using System.Threading.Tasks;

namespace Shop.Application.Emails
{
    public interface IEmailSink
    {
        ValueTask SendAsync(SendEmailRequest request);
    }
}
