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
        private readonly SellOrderService _orderService; 
        private readonly OrderDetailService _orderDetailService;

        //public ObservableCollection<Order> OrderList { get; set; }


        public SellOrdersUI(ProductService productService, GoldService goldService,
            OrderDetailService orderDetailService, SellOrderService orderService)
        {
            _productService = productService;
            _goldService = goldService;
            _orderDetailService = orderDetailService;
            _orderService = orderService;
            InitializeComponent();
            //OrderList = new ObservableCollection<Order>();
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            var orders = GetOrders(); 
            dgSellOrders.ItemsSource = orders;
        }

        private List<SellOrderDto> GetOrders()
        {
            List<SellOrderDto> orders = _orderService.GetSellOrders();
            return orders;
        }

    }
}
