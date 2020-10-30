
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Emails;

namespace Shop.UI.Workers.Email
{
    public static class RegisterEmailService
    {
        public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<EmailSettings>(config.GetSection("Email"));
            services.AddSingleton<IEmailTemplateFactory, EmailTemplateFactory>();
            services.AddSingleton<EmailChannel>();
            services.AddSingleton<IEmailSink>(p => p.GetRequiredService<EmailChannel>());
            services.AddSingleton<IEmailQueue>(p => p.GetRequiredService<EmailChannel>());
            services.AddHostedService<EmailService>();
            return services;
        }
    }
}
