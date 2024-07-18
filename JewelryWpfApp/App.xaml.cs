using JewelryWpfApp.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Repositories;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Navigation;

namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();



            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"appsettings.json", optional: true, reloadOnChange: true);
            var config = builder.Build();

            serviceCollection.AddApplicationServices(config);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            // update db and add seed data to db
            var dataContext = ServiceProvider.GetRequiredService<DataContext>();
            SeedDataAndMigrate(dataContext);


            var loginWindow = ServiceProvider.GetRequiredService<Login>();
            loginWindow.Show();
        }

        private void SeedDataAndMigrate(DataContext dataContext)
        {
            try
            {
                if (dataContext.Database.GetPendingMigrations().Count() > 0)
                {
                    dataContext.Database.Migrate();
                }
            }
            catch (Exception ex) { }
            DataContextSeed.SeedData(dataContext);
        }
    }

}
