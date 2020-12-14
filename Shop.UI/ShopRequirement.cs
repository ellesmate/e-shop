using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.UI
{
    public class ShopRequirement : ClaimsAuthorizationRequirement, IAuthorizationRequirement
    {
        public ShopRequirement(string claimType, IEnumerable<string> allowedValues) : base(claimType, allowedValues) { }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimsAuthorizationRequirement requirement)
        {
            if (context.User != null)
            {
                if (context.User.HasClaim(ShopConstants.Claims.Role, ShopConstants.Roles.Admin))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    return base.HandleRequirementAsync(context, requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
