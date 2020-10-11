using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Shop.Database;
using Shop.Domain.Models;

namespace Shop.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            try
            {
                using (var scope = host.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                    context.Database.EnsureCreated();

                    if (!context.Users.Any())
                    {
                        var adminUser = new User
                        {
                            UserName = "Admin",
                            EmailConfirmed = true
                        };

                        var managerUser = new User
                        {
                            UserName = "Manager",
                            EmailConfirmed = true
                        };

                        var supportUser = new User
                        {
                            UserName = "Support",
                            EmailConfirmed = true
                        };

                        userManager.CreateAsync(adminUser, "password").GetAwaiter().GetResult();
                        userManager.CreateAsync(managerUser, "password").GetAwaiter().GetResult();
                        userManager.CreateAsync(supportUser, "password").GetAwaiter().GetResult();

                        var adminClaim = new Claim(ShopConstants.Claims.Role, ShopConstants.Roles.Admin);
                        var managerClaim = new Claim(ShopConstants.Claims.Role, ShopConstants.Roles.Manager);
                        var guestClaim = new Claim(ShopConstants.Claims.Role, ShopConstants.Roles.Guest);

                        userManager.AddClaimAsync(adminUser, adminClaim).GetAwaiter().GetResult();
                        userManager.AddClaimAsync(managerUser, managerClaim).GetAwaiter().GetResult();
                        userManager.AddClaimAsync(supportUser, guestClaim).GetAwaiter().GetResult();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, builder) =>
                {
                    if (hostContext.HostingEnvironment.IsDevelopment())
                    {
                        builder.AddUserSecrets<Program>();
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var port = Environment.GetEnvironmentVariable("PORT");
                    if (port != null)
                    {
                        webBuilder.UseUrls(new string[] { $"http://*:{port}" });
                    }
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging((hostingContext, logging)=>
                {
                    logging.ClearProviders();
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                })
                .UseNLog();
    }
}
