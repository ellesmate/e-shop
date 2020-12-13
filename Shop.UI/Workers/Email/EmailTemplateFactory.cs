using DotLiquid;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Shop.Application.Orders;
using Shop.Application.Emails;
using Shop.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Shop.Application.Projections;

namespace Shop.UI.Workers.Email
{
    public class EmailTemplateFactory : IEmailTemplateFactory
    {
        private readonly IWebHostEnvironment _env;
        private Dictionary<string, Template> TemplateCache { get; } = new Dictionary<string, Template>();

        public EmailTemplateFactory(IWebHostEnvironment env)
        {
            _env = env;
        }

        public Task<string> RenderOrderConfirmationAsync(Order order) =>
            Compose(
                RenderHeaderAsync(),
                RenderTemplateAsync("order-confirmation", GetOrder.Projection(order)),
                RenderFooterAsync()
            );

        public Task<string> RenderAccountConfirmationAsync(DomainUser user, string link) => 
            Compose(
                RenderHeaderAsync(),
                RenderTemplateAsync("account-confirmation", new { User = UserProjection.Project(user), Link = link }),
                RenderFooterAsync()
            );
        public Task<string> RenderAccountResetPasswordAsync(DomainUser user, string link) =>
            Compose(
                RenderHeaderAsync(),
                RenderTemplateAsync("reset-password", new { User = UserProjection.Project(user), Link = link }),
                RenderFooterAsync()
            );

        private Task<string> RenderHeaderAsync() => RenderTemplateAsync("header");
        private Task<string> RenderFooterAsync() => RenderTemplateAsync("footer");

        private static async Task<string> Compose(params Task<string>[] components) =>
            string.Concat(await Task.WhenAll(components));

        private async Task<string> RenderTemplateAsync(string templateName, object seed = null)
        {
            var templatePath = Path.Combine(_env.WebRootPath, "email-templates", $"{templateName}.liquid");
            if (_env.IsDevelopment() || !TemplateCache.TryGetValue(templatePath, out var template))
            {
                var templateString = await File.ReadAllTextAsync(templatePath);
                TemplateCache[templatePath] = template = Template.Parse(templateString);
            }

            return template.Render(Hash.FromAnonymousObject(seed));
        }
    }
}
