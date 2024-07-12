using Services.Dto;
using Services;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Repositories.Entities;

namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for PurchaseOrdersUI.xaml
    /// </summary>
    public partial class PurchaseOrdersUI : Page
    {
        private readonly ProductService _productService;
        private readonly GoldService _goldService;
        private readonly PurchaseOrderService _purchaseOrderService; 
        private readonly OrderDetailService _orderDetailService;
        private readonly UserSessionService _userSessionService;
        private readonly CustomerService _customerService;
        private PurchaseOrderDto? _selected = null;

        public PurchaseOrdersUI(ProductService productService, GoldService goldService,
            PurchaseOrderService purchaseOrderService, OrderDetailService orderDetailService,
            UserSessionService userSessionService, CustomerService customerService)
        {
            _productService = productService;
            _goldService = goldService;
            _purchaseOrderService = purchaseOrderService;
            _orderDetailService = orderDetailService;
            _userSessionService = userSessionService;
            _customerService = customerService;
            InitializeComponent();
        }

        private async void PageLoaded(object sender, RoutedEventArgs e)
        {
            await FillDataGridView();
        }

        private async Task FillDataGridView()
        {
            IEnumerable<PurchaseOrderDto> products = await _purchaseOrderService.GetPurchaseOrders();
            dgvPurchaseOrders.ItemsSource = products;
        }

        private async void dgvPurchaseOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvPurchaseOrders.SelectedItems.Count > 0)
            {
                try
                {
                    _selected = (PurchaseOrderDto)dgvPurchaseOrders.SelectedItems[0];
                    PurchaseOrderDetailUI purchaseOrderDetailUI 
                        = new PurchaseOrderDetailUI(
                            _productService
                            , _goldService
                            , _orderDetailService
                            , _purchaseOrderService
                            , _userSessionService
                            , _customerService);
                    purchaseOrderDetailUI.purchaseOrderDto = _selected;
                    purchaseOrderDetailUI.ShowDialog();

                    await FillDataGridView();
                }
                catch
                {
                    MessageBox.Show("Please select a valid row!",
                    "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            else
            {
                _selected = null;
            }
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var searchValue = txtSearch.Text;

            dgvPurchaseOrders.ItemsSource = await _purchaseOrderService.GetPurchaseOrdersByDate(searchValue);
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            PurchaseOrderDetailUI purchaseOrderDetailUI 
                = new PurchaseOrderDetailUI(
                    _productService
                    , _goldService
                    , _orderDetailService
                    , _purchaseOrderService
                    , _userSessionService
                    , _customerService);
            purchaseOrderDetailUI.ShowDialog();
            await FillDataGridView();
        }

    }
}
