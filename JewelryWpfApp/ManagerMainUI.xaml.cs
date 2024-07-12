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
        private readonly PurchaseOrdersUI _purchaseOrdersUI;
        private readonly GoldRateUI _goldRateUI;
        private readonly SellOrdersUI _sellOrdersUI;
        IServiceProvider _serviceProvider;
        public ManagerMainUI(ProductsListUI productListUI, GoldRateUI goldRateUI, IServiceProvider serviceProvider, SellOrdersUI sellOrdersUI)
        {
            _productsListUI = productListUI;
            _goldRateUI = goldRateUI;
            _serviceProvider = serviceProvider;
            _purchaseOrdersUI = purchaseOrdersUI;
            InitializeComponent();
            _sellOrdersUI = sellOrdersUI;
        }

        /* Gold Page is shown first*/
        private void StartWindow(object sender, EventArgs e)
        {
            // frMain.Content = ...
        }

        /* Navigate to gold price management page*/
        private void btnNavGold_Click(object sender, RoutedEventArgs e)
        {
            frMain.Content = _goldRateUI;
        }

        /* Navigate to product management page*/
        private void btnNavProduct_Click(object sender, RoutedEventArgs e)
        {
            frMain.Content = _productsListUI;
        }

        /* Navigate to sales_order management page*/
        private void btnNavSalesOrder_Click(object sender, RoutedEventArgs e)
        {
            frMain.Content =_sellOrdersUI;
        }
       
        /* Navigate to purchase_order management page*/
        private void btnNavPurchaseOrder_Click(object sender, RoutedEventArgs e)
        {
            frMain.Content = _purchaseOrdersUI;
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