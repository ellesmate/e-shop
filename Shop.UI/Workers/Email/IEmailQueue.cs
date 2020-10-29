using Shop.Application.Emails;
using System.Threading.Tasks;

namespace Shop.UI.Workers.Email
{
    public interface IEmailQueue
    {
        ValueTask<SendEmailRequest> ReadAsync();
    }
}
