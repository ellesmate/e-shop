using System.Security.Claims;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NETCore.MailKit.Core;
using Shop.Domain.Models;

namespace Shop.UI.Pages.Accounts
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
        }

        [BindProperty]
        public RegisterViewModel Input { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var user = new User 
            {
                UserName = Input.Username,
                Email = Input.Email
            };
            
            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded) 
            {

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var link = Url.Action("VerifyEmail", "Accounts", new { userId = user.Id, code }, Request.Scheme, Request.Host.ToString());

                await _emailService.SendAsync(Input.Email, "email verify", $"<a href=\"{link}\">Verify Email</a>", true);

                return RedirectToPage("/Accounts/Login");
            }

            return Page();
        }

        public class RegisterViewModel
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}