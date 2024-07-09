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
using System.Collections.ObjectModel;

namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for SellOrdersUI.xaml
    /// </summary>
    public partial class SellOrdersUI : Page
    {
        private readonly ProductService _productService;
        private readonly GoldService _goldService;
        private readonly OrderService _orderService; 
        private readonly OrderDetailService _orderDetailService;

        public ObservableCollection<Order> OrderList { get; set; }


        public SellOrdersUI(ProductService productService, GoldService goldService,
            OrderDetailService orderDetailService)
        {
            _productService = productService;
            _goldService = goldService;
            _orderDetailService = orderDetailService;
            InitializeComponent();
            OrderList = new ObservableCollection<Order>();
        }

        private async void PageLoaded(object sender, RoutedEventArgs e)
        {
            await FillDataGridView();
        }

        private async Task FillDataGridView()
        {
            IEnumerable<Order> orders = _orderService.GetSellOrders(); 
            OrderList.Clear(); 
            foreach (var order in orders)
            {
                OrderList.Add(order); 
            }
        }

    }
}
