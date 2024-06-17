using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for ManagerMainUI.xaml
    /// </summary>
    public partial class ManagerMainUI : Window
    {
        private readonly ProductsListUI _productsListUI;
        IServiceProvider _serviceProvider;
        public ManagerMainUI(ProductsListUI productListUI, IServiceProvider serviceProvider)
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

        /* Navigate to gold price management page*/
        private void btnNavGold_Click(object sender, RoutedEventArgs e)
        {
            
        }

        /* Navigate to product management page*/
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

        /* Navigate to staff management page*/
        private void btnNavStaff_Click(object sender, RoutedEventArgs e)
        {

        }

        /* Navigate to profile page*/
        private void btnNavProfile_Click(object sender, RoutedEventArgs e)
        {

        }

        /* Logout */
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = _serviceProvider.GetRequiredService<Login>();
            loginWindow.Show();
            Close();
        }
    }
}