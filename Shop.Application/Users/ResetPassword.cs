using Microsoft.AspNetCore.Identity;
using Shop.Domain.Models;
using System.Threading.Tasks;

namespace Shop.Application.Users
{
    public class ResetPassword
    {
        private readonly UserManager<User> _userManager;

        public ResetPassword(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> DoAsync(Request request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user is null)
            {
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Code, request.Password);
            return result.Succeeded;
        }

        public class Request
        {
            public string UserId { get; set; }
            public string Code { get; set; }
            public string Password { get; set; }
        }
    }
}
