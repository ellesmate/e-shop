
namespace Shop.UI
{
    public struct ShopConstants
    {
        public struct Policies
        {
            public const string Admin = nameof(Admin);
            public const string Manager = nameof(Manager);
            public const string Customer = nameof(Customer);
            public const string Support = nameof(Support);
        }
        public struct Schemas
        {
            public const string Guest = "GuestCookie";
        }

        public struct Claims
        {
            public const string Role = nameof(Role);
        }

        public struct Roles
        {
            public const string Customer = nameof(Customer);
            public const string Admin = nameof(Admin);
            public const string Manager = nameof(Manager);
            public const string Support = nameof(Support);
        }
    }
}
