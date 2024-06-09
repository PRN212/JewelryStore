using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.Logging;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OrderManagementUI _orderManagementUI;
        ReceiptManagementUI _receiptManagementUI;
        IServiceProvider _serviceProvider;

        public MainWindow(IServiceProvider serviceProvider, OrderManagementUI orderManagementUI, ReceiptManagementUI receiptManagementUI)
        {
            _orderManagementUI = orderManagementUI;
            _receiptManagementUI = receiptManagementUI;
            _serviceProvider = serviceProvider;
            InitializeComponent();
        }

        public MainWindow()
        {

        }

        private void StartWindow(object sender, EventArgs e)
        {
            frMain.Content = _orderManagementUI;
        }

        private void btnNavOrder_Click(object sender, RoutedEventArgs e)
        {
            frMain.Content = _orderManagementUI;
        }

        private void btnNavReceipt_Click(object sender, RoutedEventArgs e)
        {
            frMain.Content = _receiptManagementUI;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            //var loginWindow = _serviceProvider.GetRequiredService<Login>();
            //loginWindow.Show();
        }
    }
}