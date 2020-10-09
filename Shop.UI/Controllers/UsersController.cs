using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.UI.ViewModels.Admin;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shop.UI.Controllers
{
    [Route("[controller]")]
    [Authorize(Policy = ShopConstants.Policies.Admin)]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> CreateUser([FromBody] CreateUserViewModel vm)
        {
            var managerUser = new IdentityUser
            {
                UserName = vm.Username,
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(managerUser, "password");

            var managerClaim = new Claim(ShopConstants.Claims.Role, ShopConstants.Roles.Manager);

            await _userManager.AddClaimAsync(managerUser, managerClaim);

            return Ok();
        }

    }
}
