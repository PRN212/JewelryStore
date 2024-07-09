

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repositories.Entities;

namespace Repositories
{
    public class DataContext : DbContext
    {
        public DataContext() { }
        //public DataContext(DbContextOptions options) : base(options)
        //{
        //}
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Gold> Golds { get; set; }
        public virtual DbSet<GoldPrice> GoldPrices { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>()
                .HasKey(k => new { k.OrderId, k.ProductId });
        }

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
    }
}
