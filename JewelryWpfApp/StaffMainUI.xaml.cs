using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for StaffMainUI.xaml
    /// </summary>
    public partial class StaffMainUI : Window
    {
        IServiceProvider _serviceProvider;
        private readonly ProductsListUI _productsListUI;
        
        public StaffMainUI(ProductsListUI productListUI, IServiceProvider serviceProvider)
        {
            _productsListUI = productListUI;           
            _serviceProvider = serviceProvider;
            InitializeComponent();
        }

        /* Gold Page is shown first*/
        private void StartWindow(object sender, EventArgs e)
        {
            // frMain.Content = ...
        }

        /* Navigate to gold price page*/
        private void btnNavGold_Click(object sender, RoutedEventArgs e)
        {

        }

        /* Navigate to product list page*/ 
        private void btnNavProduct_Click(object sender, RoutedEventArgs e)
        {
            frMain.Content = _productsListUI;
        }

        /* Navigate to sales_order management page*/
        private void btnNavSalesOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        /* Navigate to purchase_order management page*/
        private void btnNavPurchaseOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        /* Navigate to profile page*/
        private void btnNavProfile_Click(object sender, RoutedEventArgs e)
        {

        }

        /*Logout*/
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = _serviceProvider.GetRequiredService<Login>();
            loginWindow.Show();
            Close();
        }
    }
}
