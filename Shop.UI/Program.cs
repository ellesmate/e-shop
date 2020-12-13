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
using Shop.Database.Models;
using Shop.Domain.Enums;

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
                            Stocks = new List<Stock>
                            {
                                new Stock {Description = "Default", Qty = 100,},
                            },
                            Images = new List<Image>
                            {
                                new Image {Index = 0, Path = "https://e-shopdotnet-bucket.s3.eu-central-1.amazonaws.com/static/images/637434848106787046_0.jpg"},
                            }
                        });

                        context.Add(new Product
                        {
                            Name = "MD SDG653",
                            Slug = "md-sdg653",
                            Description = "Количество струн: 6. Форма: джаз. Верхняя дека: ель. Нижняя дека и обечайка: махагонь. Накладка грифа: палисандр. Цвет верхней деки: натуральный. Покрытие: глянцевое. Механизм крепления струн: металлический держатель. Звукосниматели: 1 single. Элементы регулировки: звук и тон. Форма резонаторных отверстий: f-образная",
                            Value = 454.54M,
                            Category = Category.Electric,
                            Stocks = new List<Stock>
                            {
                                new Stock {Description = "Default", Qty = 100,},
                            },
                            Images = new List<Image>
                            {
                                new Image {Index = 0, Path = "https://e-shopdotnet-bucket.s3.eu-central-1.amazonaws.com/static/images/637434848875781471_0.jpg"},
                            }
                        });
                        context.Add(new Product
                        {
                            Name = "Cort Sunset NY",
                            Slug = "cort-sunset-ny",
                            Description = "Электро-классическая гитара/ Корпус - красное дерево с полостями / Передняя дека --- массив ели / Гриф - красное дерево / Накладка на гриф - палисандр / Датчик --- B-Band A11",
                            Value = 915.20M,
                            Category = Category.Electric,
                            Stocks = new List<Stock>
                            {
                                new Stock {Description = "Default", Qty = 100,},
                            },
                            Images = new List<Image>
                            {
                                new Image {Index = 0, Path = "https://e-shopdotnet-bucket.s3.eu-central-1.amazonaws.com/static/images/637434848985314941_0.jpg"},
                            }
                        });

                        context.Add(new Product
                        {
                            Name = "Epiphone Dot SB",
                            Slug = "epiphone-dot-sb",
                            Description = "полуакустическая электрогитара, цвет санберст, корпус ламинированный клён, вклееный гриф махогон, накладка грифа палисандр, инкрустация в виде точек, мензура 24,75`, ширина верхнего порожка 42 мм, звукосниматели H-H, регулировки - 2 громкости, 2 тона, 3х-поз.переключатель, фурнитура - хром, бридж — Tune-o-Matic.",
                            Value = 1075.50M,
                            Category = Category.Electric,
                            Stocks = new List<Stock>
                            {
                                new Stock {Description = "Default", Qty = 100,},
                            },
                            Images = new List<Image>
                            {
                                new Image {Index = 0, Path = "https://e-shopdotnet-bucket.s3.eu-central-1.amazonaws.com/static/images/637434849078117060_0.jpg"},
                            }
                        });

                        context.Add(new Product
                        {
                            Name = "IBANEZ AS93",
                            Slug = "ibanez-as93",
                            Description = "Полуакустическая электрогитара Ibanez AS93. Количество ладов: 22. Корпус: волнистый клен. Гриф: 3-кусочный махагон/клен, вклеенный. Накладка: палисандр. Струнодержатель: Quik Change III регулируемый. Звукосниматели: H/H (ACH1/ACH2). Ширина верхнего порожка: 43 мм. Фурнитура: Gold. Цвет: Violin Sunburst.",
                            Value = 1556.00M,
                            Category = Category.Electric,
                            Stocks = new List<Stock>
                            {
                                new Stock {Description = "Default", Qty = 100,},
                            },
                            Images = new List<Image>
                            {
                                new Image {Index = 0, Path = "https://e-shopdotnet-bucket.s3.eu-central-1.amazonaws.com/static/images/637434856198085207_0.jpg"},
                                new Image {Index = 1, Path = "https://e-shopdotnet-bucket.s3.eu-central-1.amazonaws.com/static/images/637434856201395236_1.jpg"},
                                new Image {Index = 2, Path = "https://e-shopdotnet-bucket.s3.eu-central-1.amazonaws.com/static/images/637434856201441802_2.jpg"},
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
