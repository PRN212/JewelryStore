using Microsoft.Extensions.DependencyInjection;
using Repositories.Entities;
using System.Windows;


namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for ManagerMainUI.xaml
    /// </summary>
    public partial class ManagerMainUI : Window
    {
        private readonly ProductsListUI _productsListUI;
        private readonly PurchaseOrdersListUI _purchaseOrdersUI;
        private readonly GoldRateUI _goldRateUI;
        private readonly GoldPriceUI _goldPriceUI;
        private readonly SellOrdersUI _sellOrdersUI;
        private readonly IServiceProvider _serviceProvider;
        public ManagerMainUI(ProductsListUI productListUI, GoldRateUI goldRateUI, IServiceProvider serviceProvider, 
            SellOrdersUI sellOrdersUI, PurchaseOrdersListUI purchaseOrdersUI, GoldPriceUI goldPriceUI)
        {
            _productsListUI = productListUI;
            _goldRateUI = goldRateUI;
            _serviceProvider = serviceProvider;
            _purchaseOrdersUI = purchaseOrdersUI;
            InitializeComponent();
            _sellOrdersUI = sellOrdersUI;
            _goldPriceUI = goldPriceUI;
        }

        /* Gold Page is shown first*/
        private void StartWindow(object sender, EventArgs e)
        {
            frMain.Content = _goldPriceUI;
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

        /* Logout */
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = _serviceProvider.GetRequiredService<Login>();
            loginWindow.Show();
            Close();
        }

        private void btnNavGoldPrice_Click(object sender, RoutedEventArgs e)
        {
            frMain.Content = _goldPriceUI;
        }
    }
}