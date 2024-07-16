using Services.Dto;
using Services;
using System.Windows;
using System.Windows.Controls;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using Repositories.Entities.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for PurchaseOrdersUI.xaml
    /// </summary>
    public partial class PurchaseOrdersListUI : Page
    {
        private readonly PurchaseOrderService _purchaseOrderService; 
        private readonly IServiceProvider _serviceProvider;
        private PurchaseOrderDto? _selected = null;

        public PurchaseOrdersListUI(PurchaseOrderService purchaseOrderService, IServiceProvider serviceProvider)
        {
            _purchaseOrderService = purchaseOrderService;
            _serviceProvider = serviceProvider;
            InitializeComponent();        
        }

        private async void PageLoaded(object sender, RoutedEventArgs e)
        {
            await FillDataGridView();
        }

        private async Task FillDataGridView()
        {
            // set value for combobox of order status
            cbOrderStatus.ItemsSource = new List<string> {
                OrderStatus.Pending.GetEnumMemberValue(), 
                OrderStatus.Cancel.GetEnumMemberValue(),
                OrderStatus.PaymentReceived.GetEnumMemberValue()};

            IEnumerable<PurchaseOrderDto> purchaseOrders = await _purchaseOrderService.GetOrdersWithSpec
                ("",OrderType.Purchase.GetEnumMemberValue(),"");
            dgvPurchaseOrders.ItemsSource = purchaseOrders;
        }

        private async void dgvPurchaseOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvPurchaseOrders.SelectedItems.Count > 0)
            {
                _selected = (PurchaseOrderDto)dgvPurchaseOrders.SelectedItems[0];

                var purchaseOrderDetailUI = _serviceProvider.GetRequiredService<PurchaseOrderUI>();
                purchaseOrderDetailUI.SelectedOrder = _selected;
                purchaseOrderDetailUI.ShowDialog();

                await FillDataGridView();
            }
            else
            {
                _selected = null;
            }
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var searchValue = txtSearch.Text;
            var orderStatus = cbOrderStatus.SelectedValue.ToString();
            dgvPurchaseOrders.ItemsSource = await _purchaseOrderService.GetOrdersWithSpec
            (searchValue, OrderType.Purchase.GetEnumMemberValue(), orderStatus);
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var customerUI = _serviceProvider.GetRequiredService<CustomerUI>();
            customerUI.ShowDialog();

            await FillDataGridView();
        }

    }
}
