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
using Microsoft.Extensions.DependencyInjection;

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
		private readonly CustomerService _customerService;
		private SellOrderDto? _selected = null;
		private IServiceProvider _serviceProvider;
		//public ObservableCollection<Order> OrderList { get; set; }


		public SellOrdersUI(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			_productService = _serviceProvider.GetRequiredService<ProductService>();
			_goldService = _serviceProvider.GetRequiredService<GoldService>();
			_orderDetailService = _serviceProvider.GetRequiredService<OrderDetailService>();
			_orderService = _serviceProvider.GetRequiredService<SellOrderService>();
			_customerService = _serviceProvider.GetRequiredService<CustomerService>();

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

		private void dgSellOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (dgSellOrders.SelectedItems.Count > 0)
			{
				try
				{
					_selected = (SellOrderDto)dgSellOrders.SelectedItems[0];
					//var details = _orderDetailService.GetDetailsFromOrder(_selected.Id);
					UpsertSellOrderDetailUI upsertSellOrderDetailUI = new UpsertSellOrderDetailUI(_serviceProvider, _selected.Id);
					upsertSellOrderDetailUI.ShowDialog();
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

			dgSellOrders.ItemsSource = _orderService.GetByCustomerName(searchValue);
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
