using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Emails;
using Shop.Database.Models;
using Shop.Domain.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Shop.UI.Infrastructure
{
    public class AccountManager
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSink _emailSink;
        private readonly IEmailTemplateFactory _emailTemplateFactory;
        private readonly IUrlHelper _urlHelper;
        private readonly HostSettings _hostSettings;

        public AccountManager(
            UserManager<User> userManager,
            IEmailSink emailSink,
            IEmailTemplateFactory emailTemplateFactory,
            IUrlHelper urlHelper,
            HostSettings hostSettings)
        {
            _userManager = userManager;
            _emailSink = emailSink;
            _emailTemplateFactory = emailTemplateFactory;
            _urlHelper = urlHelper;
            _hostSettings = hostSettings;
        }

        private string GenerateUrl(string controller, string action, object query)
        {
            return _urlHelper.Action(action, controller, query, _hostSettings.Scheme, _hostSettings.Host);
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
                var link = GenerateUrl("Accounts", "VerifyEmail", new { userId = user.Id, code });
                
                var emailMessage = await _emailTemplateFactory.RenderAccountConfirmationAsync(EntityUserToDomainUser(user), link);

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

            var emailMessage = await _emailTemplateFactory.RenderAccountResetPasswordAsync(EntityUserToDomainUser(user), code);
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

        public async Task<bool> VerifyEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return false;
            } 

            var result = await _userManager.ConfirmEmailAsync(user, code);
            return result.Succeeded;
        }

        private static DomainUser EntityUserToDomainUser(User user) =>
            new DomainUser
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
            };
    }
}
