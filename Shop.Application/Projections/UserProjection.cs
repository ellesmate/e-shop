using Shop.Domain.Models;

namespace Shop.Application.Projections
{
    public class UserProjection
    {
        public static object Project(DomainUser user) => new
        {
            user.Username,
            user.Email
        };
    }
}
