using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Domain.Models;
using Shop.UI.Infrastructure;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shop.UI.Controllers
{
    [AllowAnonymous]
    public class AccountsController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly AccountManager _accountManager;

        public AccountsController(
            SignInManager<User> signInManager, 
            UserManager<User> userManager,
            AccountManager accountManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _accountManager = accountManager;
        }
        
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            var result = await _accountManager.VerifyEmailAsync(userId, code);

            if (result)
            {
                return RedirectToPage("/Accounts/Login");
            }

            return BadRequest();
        }

        public IActionResult ExternalLogin()
        {
            var redirectUrl = Url.Action("ExternalResponse", "Accounts");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);

            return new ChallengeResult("Google", properties);
        }

        public async Task<IActionResult> ExternalResponse()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info is null)
                return RedirectToPage("/Accounts/Login");

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            if (result.Succeeded)
            {
                return RedirectToPage("/Index");
            }
            else if (email is null)
            {
                return RedirectToPage("/Accounts/Login");
            }
           
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                user = new User
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(user);
            }

            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, false);

            return RedirectToPage("/Index");
        }
    }
}
