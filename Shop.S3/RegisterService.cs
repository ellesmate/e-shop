using System;
using Shop.S3;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RegisterService
    {
        public static IServiceCollection AddEShopS3Client(
            this IServiceCollection services,
            Func<S3StorageSettings> settingsFactory)
        {
            services.AddSingleton(settingsFactory());
            services.AddScoped<S3Client>();

            return services;
        }
    }
}
