using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Services;

namespace JewelryWpfApp.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices (this IServiceCollection services, IConfiguration config)
        {
            // DbContext
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DBDefault"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure());
            });
            // Register Services and ViewModels
            services.AddScoped<ProductService>();
            services.AddScoped<UserService>();
            services.AddScoped<UserRepository>();
            services.AddScoped<ProductRepository>();
            services.AddScoped<GoldPriceRepository>();

            services.AddTransient<Login>();
            services.AddTransient<ManagerMainUI>();
            services.AddTransient<StaffMainUI>();
            services.AddTransient<ProductsListUI>();

            // AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
