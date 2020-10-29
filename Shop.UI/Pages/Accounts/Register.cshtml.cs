using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Users;

namespace Shop.UI.Pages.Accounts
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegisterViewModel Input { get; set; }
        public void OnGet() {}

        public async Task<IActionResult> OnPost([FromServices] RegisterUser registerUser)
        {
            var result = await registerUser.DoAsync(new RegisterUser.Request
            {
                Username = Input.Username,
                Email = Input.Email,
                Password = Input.Password
            });

            if (result) 
            {
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