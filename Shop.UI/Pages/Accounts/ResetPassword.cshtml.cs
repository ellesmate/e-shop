using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Users;

namespace Shop.UI.Pages.Accounts
{
    public class ResetPasswordModel : PageModel
    {
        [BindProperty]
        public ResetPasswordViewModel Input { get; set; }

        public void OnGet() {}

        public async Task<IActionResult> OnPost([FromServices] GenerateResetPassword generateResetPassword)
        {
            var result = await generateResetPassword.DoAsync(new GenerateResetPassword.Request
            {
                Email = Input.Email
            });

            if (result)
            {
                return Redirect("/");
            }

            return BadRequest();
        }

        public class ResetPasswordViewModel
        {
            public string Email { get; set; }
        }
    }
}
