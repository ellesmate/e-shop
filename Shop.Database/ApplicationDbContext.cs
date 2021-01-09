using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Shop.Database.Models;
using System;

namespace Shop.Database
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<OrderStock> OrderStocks { get; set; }
        public DbSet<StockOnHold> StocksOnHold { get; set; }
        public DbSet<Image> Images { get; set; }

        
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<OrderStock>()
                .HasKey(x => new { x.StockId, x.OrderId });
            builder.Entity<ChatUser>()
                .HasKey(x => new { x.ChatId, x.UserId });
        }

        public static string GetNpgsqlConnectionString(string databaseUrl, bool dev)
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

            if (!dev)
            {
                builder.Pooling = true;
                builder.SslMode = SslMode.Require;
                builder.TrustServerCertificate = true;
            }

            return builder.ToString();
        }
    }
}
