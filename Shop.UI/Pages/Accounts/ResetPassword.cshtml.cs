using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Internal;
using NETCore.MailKit.Core;
using Shop.Domain.Models;

namespace Shop.UI.Pages.Accounts
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public ResetPasswordModel(
            UserManager<User> userManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        [BindProperty]
        public ResetPasswordViewModel Input { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            var user = await _userManager.FindByEmailAsync(Input.Email);
            
            if (user == null) return BadRequest();

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            var link = Url.Action("SetPassword", "Accounts", new { userId = user.Id, code }, Request.Scheme, Request.Host.ToString());
            
            await _emailService.SendAsync("test@test.com", "Reset password", $"<a href=\"{link}\">Reset password</a>", true);

            //_userManager.ResetPasswordAsync()
            return RedirectToPage("/");
        }

        public class ResetPasswordViewModel
        {
            public string Email { get; set; }
        }
    }
}
