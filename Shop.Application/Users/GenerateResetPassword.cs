using Microsoft.AspNetCore.Identity;
using Shop.Application.Emails;
using Shop.Domain.Models;
using System.Threading.Tasks;

namespace Shop.Application.Users
{
    public class GenerateResetPassword
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSink _emailSink;
        private readonly IEmailTemplateFactory _emailTemplateFactory;

        public GenerateResetPassword(
            UserManager<User> userManager,
            IEmailSink emailSink,
            IEmailTemplateFactory emailTemplateFactory)
        {
            _userManager = userManager;
            _emailSink = emailSink;
            _emailTemplateFactory = emailTemplateFactory;
        }

        public async Task<bool> DoAsync(Request request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return false;
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var emailMessage = await _emailTemplateFactory.RenderAccountResetPasswordAsync(user, code);
            await _emailSink.SendAsync(new SendEmailRequest
            {
                To = user.Email,
                Subject = "E-shopdotnet Reset Password",
                Message = emailMessage,
            });

            return true;
        }

        public class Request
        {
            public string Email { get; set; }
        }
    }
}
