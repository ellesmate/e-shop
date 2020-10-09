using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shop.Database;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Application;
using Shop.Domain.Infrastructure;
using Shop.UI.Infrastructure;
using FluentValidation.AspNetCore;
using FluentValidation;
using Shop.Application.Cart;
using Shop.UI.ValidationContexts;
using Npgsql;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.UI
{
    public class Startup
    {
        
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
            Configuration = configuration;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddDbContext<ApplicationDbContext>(options => 
            {
                var databaseUrl = Configuration["DATABASE_URL"];
                var connectionString = GetNpgsqlConnectionString(databaseUrl);
                options.UseNpgsql(connectionString);
            });

            services.AddIdentity<IdentityUser, IdentityRole>(options => 
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            
            services.AddControllersWithViews();
            services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Admin", ShopConstants.Policies.Manager);
                    options.Conventions.AuthorizePage("/Admin/ConfigureUsers", ShopConstants.Policies.Admin);
                    options.Conventions.AuthorizeFolder("/Checkout");
                    //options.Conventions.AllowAnonymousToPage("/Admin/Login");
                })
                .AddFluentValidation(x => x.RegisterValidatorsFromAssembly(typeof(Startup).Assembly));

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(30);

                options.LoginPath = "/Accounts/Login";
                //options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                //options.SlidingExpiration = true;
            });

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    var auth = Configuration.GetSection("Authentication");

                    options.ClientId = auth["Google_ClientId"];
                    options.ClientSecret = auth["Google_ClientSecret"];
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                });

            services.AddMailKit(config => config.UseMailKit(Configuration.GetSection("Email").Get<MailKitOptions>()));

            services.AddAuthorization(options =>
            {
                options.AddPolicy(ShopConstants.Policies.Admin, policy => policy
                    .RequireClaim(ShopConstants.Claims.Role, ShopConstants.Roles.Admin));

                options.AddPolicy(ShopConstants.Policies.Manager, policy => policy
                    .AddRequirements(new ShopRequirement(ShopConstants.Claims.Role, new[] { ShopConstants.Roles.Manager })));

                options.AddPolicy(ShopConstants.Policies.Customer, policy => policy
                    .AddRequirements(new ShopRequirement(ShopConstants.Claims.Role, new[] { ShopConstants.Roles.Guest, ShopConstants.Roles.Manager })));
            });

            services.AddSession(options =>
            {
                options.Cookie.Name = "Cart";
                options.Cookie.MaxAge = TimeSpan.FromMinutes(20);
            });

            StripeConfiguration.ApiKey = Configuration.GetSection("STRIPE")["SECRET_KEY"];

            services.AddApplicationServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });

        }

        public string GetNpgsqlConnectionString(string databaseUrl)
        {
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/')
            };

            if (!WebHostEnvironment.IsDevelopment())
            {
                builder.Pooling = true;
                builder.SslMode = SslMode.Require;
                builder.TrustServerCertificate = true;
            }

            return builder.ToString();
        }

        public class ShopRequirement : ClaimsAuthorizationRequirement, IAuthorizationRequirement
        {
            public ShopRequirement(string claimType, IEnumerable<string> allowedValues) : base(claimType, allowedValues) {}

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
}
