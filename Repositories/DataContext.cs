

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repositories.Entities;
using Static;

namespace Repositories
{
    public class DataContext : DbContext
    {
        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Gold> Golds { get; set; }
        public virtual DbSet<GoldPrice> GoldPrices { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("DBDefault");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>()
                .HasKey(k => new { k.OrderId, k.ProductId });

            //Seed Orders
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "John Doe", Phone = "0123456789", Address = "3 Nam Ky Khoi Nghia" }
            );

            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, CustomerId = 1, TotalPrice = 1000f, CreatedDate = DateTime.Now, Status = "Pending", Type = SD.TypeSell, PaymentMethod = SD.TypeCredit, UserId = 1 },
                new Order { Id = 2, CustomerId = 1, TotalPrice = 2000f, CreatedDate = DateTime.Now, Status = "Completed", Type = SD.TypeSell, PaymentMethod = SD.TypeCash, UserId = 1 },
                new Order { Id = 3, CustomerId = 1, TotalPrice = 3000f, CreatedDate = DateTime.Now, Status = "Shipped", Type = SD.TypeSell, PaymentMethod = SD.TypeCash, UserId = 1 }
            );

            modelBuilder.Entity<OrderDetail>().HasData(
                new OrderDetail { OrderId = 1, ProductId = 1, Quantity = 2, Price = 500f },
                new OrderDetail { OrderId = 2, ProductId = 2, Quantity = 3, Price = 700f },
                new OrderDetail { OrderId = 3, ProductId = 3, Quantity = 4, Price = 900f }
            );
        }
    }
}
