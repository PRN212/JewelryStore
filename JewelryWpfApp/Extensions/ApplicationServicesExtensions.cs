using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Repositories.IRepositories;
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
            services.AddScoped<GoldService>(); 
            services.AddTransient<PurchaseOrderService>(); 
            services.AddScoped<OrderDetailService>(); 
            services.AddScoped<GoldPriceService>();
            services.AddScoped<CustomerService>();
            services.AddScoped<OrderDetailService>();
            services.AddScoped<SellOrderService>();

            //services.AddScoped<UserRepository>();
            services.AddScoped<GoldPriceRepository>();
            services.AddScoped<GoldRepository>();
            services.AddScoped<OrderRepository>();
            services.AddScoped<OrderDetailRepository>();
            services.AddScoped<OrderRepository>();
            services.AddScoped<OrderDetailRepository>();
            services.AddScoped<GoldPriceRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            services.AddTransient<Login>();
            services.AddScoped<SellOrdersUI>();
            services.AddTransient<ManagerMainUI>();
            services.AddTransient<StaffMainUI>();
            services.AddTransient<ProductsListUI>();
            services.AddTransient<GoldRateUI>();
            services.AddTransient<GoldPriceUI>();
            services.AddTransient<PurchaseOrdersListUI>();
            services.AddTransient<PurchaseOrderDetailUI>();
            services.AddTransient<PurchaseOrderUI>();
            services.AddTransient<CustomerUI>();

            services.AddSingleton<UserSessionService>();

            // AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
