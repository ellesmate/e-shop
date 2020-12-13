using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Domain.Models;

namespace Shop.UI.Pages.Accounts
{
    public class LoginModel : PageModel
    {
        private SignInManager<DomainUser> _signInManager;

        public LoginModel(SignInManager<DomainUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty]
        public LoginViewModel Input { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl)
        {
            var result = await _signInManager.PasswordSignInAsync(Input.Username, Input.Password, false, false);
            
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToPage("/");
            }
            else
            {
                return Page();
            }
        }

        public class LoginViewModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
