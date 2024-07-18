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

		private readonly SellOrderService _orderService;
		private SellOrderDto? _selected = null;
		private IServiceProvider _serviceProvider;
		//public ObservableCollection<Order> OrderList { get; set; }


		public SellOrdersUI(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			_orderService = _serviceProvider.GetRequiredService<SellOrderService>();


			InitializeComponent();
			//OrderList = new ObservableCollection<Order>();
		}

		private void PageLoaded(object sender, RoutedEventArgs e)
		{
			GetOrders();
		}

		private void GetOrders()
		{
			List<SellOrderDto> orders = _orderService.GetSellOrders();
			dgSellOrders.ItemsSource = orders;
		}

		private void dgSellOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (dgSellOrders.SelectedItems.Count > 0)
			{
				try
				{
					_selected = (SellOrderDto)dgSellOrders.SelectedItems[0];
					UpdateSellOrderDetailUI window = new UpdateSellOrderDetailUI(_serviceProvider, _selected.Id);
					// Subscribe to save metadata event
					window.OrderSaved += UpsertSellOrderDetailUI_OrderSaved;
					window.Closed += (s, e) => window.OrderSaved -= UpsertSellOrderDetailUI_OrderSaved;
					window.ShowDialog();
				}
				catch
				{
					MessageBox.Show("Please select a valid row!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
				}
			}
			else
			{
				_selected = null;
			}
		}

		private void UpsertSellOrderDetailUI_OrderSaved(object? sender, EventArgs e)
		{
			GetOrders();
		}

		private async void btnSearch_Click(object sender, RoutedEventArgs e)
		{
			var searchValue = txtSearch.Text;

			dgSellOrders.ItemsSource = _orderService.GetByCustomerName(searchValue);
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			AddSellOrderDetailUI window = new AddSellOrderDetailUI(_serviceProvider);

			window.OrderSaved += UpsertSellOrderDetailUI_OrderSaved;
			window.Closed += (s, e) => window.OrderSaved -= UpsertSellOrderDetailUI_OrderSaved;
			window.ShowDialog();
		}
	}
}
