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
using Shop.Domain.Enums;
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
                        var supportClaim = new Claim(ShopConstants.Claims.Role, ShopConstants.Roles.Support);

                        userManager.AddClaimAsync(adminUser, adminClaim).GetAwaiter().GetResult();
                        userManager.AddClaimAsync(managerUser, managerClaim).GetAwaiter().GetResult();
                        userManager.AddClaimAsync(supportUser, supportClaim).GetAwaiter().GetResult();
                        
                        context.Add(new Product
                        {
                            Name = "Naranda GAG110CNA",
                            Slug = "naranda-gag110cna",
                            Description = "Гитара с вырезом Naranda CAG110CNA.",
                            Value = 182.00M,
                            Category = Category.Acoustic,
                            Stock = new List<Stock>
                            {
                                new Stock {Description = "Default", Qty = 100,},
                            },
                            Images = new List<Image>
                            {
                                new Image {Index = 0, Path = "/images/guitar_1.jpg"},
                            }
                        });

                        context.Add(new Product
                        {
                            Name = "MD SDG653",
                            Slug = "md-sdg653",
                            Description = "Количество струн: 6. Форма: джаз. Верхняя дека: ель. Нижняя дека и обечайка: махагонь. Накладка грифа: палисандр. Цвет верхней деки: натуральный. Покрытие: глянцевое. Механизм крепления струн: металлический держатель. Звукосниматели: 1 single. Элементы регулировки: звук и тон. Форма резонаторных отверстий: f-образная",
                            Value = 454.54M,
                            Category = Category.Electric,
                            Stock = new List<Stock>
                            {
                                new Stock {Description = "Default", Qty = 100,},
                            },
                            Images = new List<Image>
                            {
                                new Image {Index = 0, Path = "/images/guitar_2.jpg"},
                            }
                        });
                        context.Add(new Product
                        {
                            Name = "Cort Sunset NY",
                            Slug = "cort-sunset-ny",
                            Description = "Электро-классическая гитара/ Корпус - красное дерево с полостями / Передняя дека --- массив ели / Гриф - красное дерево / Накладка на гриф - палисандр / Датчик --- B-Band A11",
                            Value = 915.20M,
                            Category = Category.Electric,
                            Stock = new List<Stock>
                            {
                                new Stock {Description = "Default", Qty = 100,},
                            },
                            Images = new List<Image>
                            {
                                new Image {Index = 0, Path = "/images/guitar_3.jpg"},
                            }
                        });

                        context.Add(new Product
                        {
                            Name = "Epiphone Dot SB",
                            Slug = "epiphone-dot-sb",
                            Description = "полуакустическая электрогитара, цвет санберст, корпус ламинированный клён, вклееный гриф махогон, накладка грифа палисандр, инкрустация в виде точек, мензура 24,75`, ширина верхнего порожка 42 мм, звукосниматели H-H, регулировки - 2 громкости, 2 тона, 3х-поз.переключатель, фурнитура - хром, бридж — Tune-o-Matic.",
                            Value = 1075.50M,
                            Category = Category.Electric,
                            Stock = new List<Stock>
                            {
                                new Stock {Description = "Default", Qty = 100,},
                            },
                            Images = new List<Image>
                            {
                                new Image {Index = 0, Path = "/images/guitar_4.jpg"},
                            }
                        });

                        context.SaveChanges();

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
