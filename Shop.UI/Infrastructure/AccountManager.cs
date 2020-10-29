using Microsoft.AspNetCore.Identity;
using Shop.Application.Emails;
using Shop.Domain.Models;
using System.Threading.Tasks;

namespace Shop.UI.Infrastructure
{
    public class AccountManager
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSink _emailSink;
        private readonly IEmailTemplateFactory _emailTemplateFactory;

        public AccountManager(
            UserManager<User> userManager,
            IEmailSink emailSink,
            IEmailTemplateFactory emailTemplateFactory)
        {
            _userManager = userManager;
            _emailSink = emailSink;
            _emailTemplateFactory = emailTemplateFactory;
        }

        public async Task<bool> RegisterAsync(string username, string email, string password)
        {
            var user = new User
            {
                UserName = username,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var emailMessage = await _emailTemplateFactory.RenderAccountConfirmationAsync(user, code);

                await _emailSink.SendAsync(new SendEmailRequest
                {
                    To = user.Email,
                    Subject = "E-shopdotnet confirmation email",
                    Message = emailMessage,
                });

                return true;
            }

            return false;
        }

        public async Task<bool> ResetPasswordRequestAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

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

        public async Task<bool> RegisterPasswordAsync(string userId, string code, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, code, password);
            return result.Succeeded;
        }
    }
}
