using Services.Dto;
using Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;

namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for PurchaseOrderDetailUI.xaml
    /// </summary>
    public partial class PurchaseOrderUI : Window
    {
        private readonly PurchaseOrderService _purchaseOrderService;
        private readonly IServiceProvider _serviceProvider;

        public PurchaseOrderDto SelectedOrder;

        public PurchaseOrderUI(PurchaseOrderService purchaseOrderService, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _purchaseOrderService = purchaseOrderService;
            InitializeComponent();         
        }

        private void FillData()
        {
            //SelectedOrder = await _purchaseOrderService.GetOrderById(SelectedOrder.Id);
            DataContext = SelectedOrder;
        }


        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            FillData();
            SetupButton();
        }

        /*
         * Add order item into purchase order
         */
        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            PurchaseOrderDetailUI w = _serviceProvider.GetRequiredService<PurchaseOrderDetailUI>();
            w.OrderId = SelectedOrder.Id;
            w.ShowDialog();
            SelectedOrder = await _purchaseOrderService.GetOrderById(SelectedOrder.Id);
            FillData();
        }

        
        /*
         * Save order information
         */
        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {

            SelectedOrder = (PurchaseOrderDto) DataContext;
            if(await _purchaseOrderService.UpdateOrder(SelectedOrder))
            {
                MessageBox.Show("Successfully updated!!!", "Success",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                MessageBox.Show("Fail to update!!!", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel this purchase order?",
                                                       "Confirm Cancel",
                                                       MessageBoxButton.OKCancel,
                                                       MessageBoxImage.Question);

            if (result == MessageBoxResult.OK)
            {

                if (await _purchaseOrderService.DeleteOrder(SelectedOrder))
                {
                    MessageBox.Show("Successfully cancel purchase order.", "Success",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                    Close(); 
                } else
                {
                    MessageBox.Show("Fail to cancel!!!", "Error",
                                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void btnPaid_Click(object sender, RoutedEventArgs e)
        {
            if (await _purchaseOrderService.IsPaid(SelectedOrder))
            {
                MessageBox.Show("Payment successful", "Success",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                MessageBox.Show("Fail to pay! Please try again!", "Error",
                                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void dgvProductsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvPurchaseOrder_ProductsList.SelectedItems.Count > 0)
            {
                
                var detailUI = _serviceProvider.GetRequiredService<PurchaseOrderDetailUI>();
                detailUI.SelectedOrderDetail = (ProductDto)dgvPurchaseOrder_ProductsList.SelectedItems[0];
                detailUI.OrderId = SelectedOrder.Id;

                detailUI.ShowDialog();
                SelectedOrder = await _purchaseOrderService.GetOrderById(SelectedOrder.Id);
                FillData();
            }
        }

        private void SetupButton()
        {
            if (SelectedOrder != null && SelectedOrder.Status != "Pending")
            {
                btnSave.IsEnabled = false;
                btnDelete.IsEnabled = false;
                btnPaid.IsEnabled = false;
                btnAdd.IsEnabled = false;

                btnSave.Foreground = new SolidColorBrush(Colors.Black);
                btnDelete.Foreground = new SolidColorBrush(Colors.Black);
                btnPaid.Foreground = new SolidColorBrush(Colors.Black);
                btnAdd.Foreground = new SolidColorBrush(Colors.Black) ;
            }
            else if (SelectedOrder == null)
            {
                btnDelete.IsEnabled = false;
                btnPaid.IsEnabled = false;
                btnDelete.Foreground = new SolidColorBrush(Colors.Black);
                btnPaid.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

    }
}
 