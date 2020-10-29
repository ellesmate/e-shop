using Microsoft.AspNetCore.Identity;
using Shop.Application.Emails;
using Shop.Domain.Models;
using System.Threading.Tasks;

namespace Shop.Application.Users
{
    [Service]
    public class RegisterUser
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSink _emailSink;
        private readonly IEmailTemplateFactory _emailTemplateFactory;

        public RegisterUser(
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
            var user = new User
            {
                UserName = request.Username,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

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

        public class Request
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
