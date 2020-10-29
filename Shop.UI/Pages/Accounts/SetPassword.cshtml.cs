using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Users;

namespace Shop.UI.Pages.Accounts
{
    public class SetPasswordModel : PageModel
    {
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

        public async Task<IActionResult> OnPost([FromServices] ResetPassword resetPassword)
        {
            var result = await resetPassword.DoAsync(new ResetPassword.Request
            {
                UserId = Input.UserId,
                Code = Input.Code,
                Password = Input.Password
            });

            if (result)
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
