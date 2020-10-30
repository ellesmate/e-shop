using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelper _urlHelper;

        public AccountManager(
            UserManager<User> userManager,
            IEmailSink emailSink,
            IHttpContextAccessor httpContextAccessor,
            IEmailTemplateFactory emailTemplateFactory,
            IUrlHelper urlHelper)
        {
            _userManager = userManager;
            _emailSink = emailSink;
            _emailTemplateFactory = emailTemplateFactory;
            _httpContextAccessor = httpContextAccessor;
            _urlHelper = urlHelper;
        }

        private string GenerateUrl(string controller, string action, object query)
        {
            var context = _httpContextAccessor.HttpContext;
            var scheme = context.Request.Scheme;
            var host = context.Request.Host.Value;

            return _urlHelper.Action(action, controller, query, scheme, host);
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
                
                var emailMessage = await _emailTemplateFactory.RenderAccountConfirmationAsync(user, link);

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
    }
}
