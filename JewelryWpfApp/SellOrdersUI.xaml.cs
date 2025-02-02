﻿using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Dto;
using System.Windows;
using System.Windows.Controls;

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
			var ordersByPhoneOrName = _orderService.GetByCustomerPhoneOrName(searchValue);
			dgSellOrders.ItemsSource = ordersByPhoneOrName;
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
