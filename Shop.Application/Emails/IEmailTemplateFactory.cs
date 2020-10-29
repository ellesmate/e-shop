using System.Threading.Tasks;
using Shop.Domain.Models;

namespace Shop.Application.Emails
{
    public interface IEmailTemplateFactory
    {
        public Task<string> RenderAccountConfirmationAsync(User user, string code);
        public Task<string> RenderAccountResetPasswordAsync(User user, string code);
        public Task<string> RenderOrderConfirmationAsync(Order order);
    }
}
