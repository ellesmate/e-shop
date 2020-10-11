using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Domain.Models;

namespace Shop.UI.Pages.Accounts
{
    public class SetPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public SetPasswordModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        [BindProperty]
        public ResetPasswordViewModel Input { get; set; }
        public void OnGet(string userId, string code)
        {
            Input = new ResetPasswordViewModel
            {
                UserId = userId,
                Code = code
            };
        }

        public async Task<IActionResult> OnPost()
        {
            var user = await _userManager.FindByIdAsync(Input.UserId);

            if (user == null) return BadRequest();

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);

            if (result.Succeeded)
            {
                return RedirectToPage("/Accounts/Login");
            }

            return BadRequest();
        }

        public class ResetPasswordViewModel
        {
            public string UserId { get; set; }
            public string Code { get; set; }
            public string Password { get; set; }
        }
    }
}
