using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Database.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shop.UI.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private SignInManager<User> _signInManager;

        public AccountsController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            var result = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, false, false);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            
            return Ok();
        }

        [HttpGet("me")]
        public IActionResult Me()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok(new { Username = User.FindFirst(ClaimTypes.Name).Value });
            }

            return BadRequest();
        }

        public class LoginViewModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
