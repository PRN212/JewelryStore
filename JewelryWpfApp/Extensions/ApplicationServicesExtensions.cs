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
            // Services and ViewModels
            services.AddScoped<ProductService>();
            services.AddTransient<Login>();

            // AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
