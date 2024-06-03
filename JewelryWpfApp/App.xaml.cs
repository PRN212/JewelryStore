using Microsoft.VisualBasic.Logging;
using Repositories;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Navigation;

namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            using (var db = new DataContext())
            {
                DataContextSeed.SeedData(db);
            }
        }
    }

}
