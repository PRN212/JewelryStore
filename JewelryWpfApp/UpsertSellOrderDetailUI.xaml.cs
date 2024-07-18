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
using System.Windows.Shapes;
using System.Xml.Linq;
using Repositories.Entities;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace JewelryWpfApp
{
	/// <summary>
	/// Interaction logic for PurchaseOrderDetailUI.xaml
	/// </summary>
	public partial class UpsertSellOrderDetailUI : Window
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly ProductService _productService;
		private readonly OrderDetailService _orderDetailService;
		private readonly SellOrderService _sellOrderService;
		private readonly CustomerService _customerService; 
		private int orderId = 0;
		private Order order;

		public UpsertSellOrderDetailUI(IServiceProvider serviceProvider, int orderId)
		{
			_serviceProvider = serviceProvider;
			_productService = _serviceProvider.GetRequiredService<ProductService>();
			_orderDetailService = _serviceProvider.GetRequiredService<OrderDetailService>();
			_sellOrderService = _serviceProvider.GetRequiredService<SellOrderService>();
			_customerService = _serviceProvider.GetRequiredService<CustomerService>();
			this.orderId = orderId;
			order = _sellOrderService.Get(order => order.Id == orderId, includeProperties: "OrderDetails", tracked: true);
			InitializeComponent();
		}

		private async void FillData()
		{
			cbCustomer.ItemsSource = await _customerService.GetAllCustomersAsync();
			cbCustomer.SelectedValuePath = "Id";

			cbProduct.ItemsSource = await _productService.GetProducts();
			cbProduct.SelectedValuePath = "Id";
			cbProduct.DisplayMemberPath = "Name";

			if (orderId == 0) return;
			var details = _orderDetailService.GetDetailsFromOrder(orderId);
			dgvSellOrder.ItemsSource = details;
			cbCustomer.SelectedValue = order.CustomerId;
		}

		private async void PageLoaded(object sender, RoutedEventArgs e)
		{
			FillData();
		}

		private async void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			if (cbProduct.SelectedItem == null)
			{
				MessageBox.Show("Please select a product.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (string.IsNullOrEmpty(txtQuantity.Text))
			{
				MessageBox.Show("Please enter a quantity.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (int.TryParse(txtQuantity.Text, out int quantity))
			{
				int productId = (int)cbProduct.SelectedValue;
				// Search for an existing OrderDetail with the same orderId and productId
				var existingDetail = order.OrderDetails.FirstOrDefault(detail => detail.OrderId == orderId && detail.ProductId == productId);

				if (existingDetail != null)
				{
					// If found, only update the quantity
					existingDetail.Quantity += quantity;
				}
				else
				{
					// If not found, create a new OrderDetail
					OrderDetail detail = new OrderDetail()
					{
						OrderId = orderId,
						ProductId = productId,
						Quantity = quantity
					};
					order.OrderDetails.Add(detail);
				}

				_sellOrderService.Update(order);
				_sellOrderService.Save();
			}
			else
			{
				MessageBox.Show("Please enter a valid quantity.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			FillData();
		}

		private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
		{
			CustomerDetail view = new(_serviceProvider);
			// Subscribe to the Closed event
			//view.Closed += CustomerDetailWindow_Closed;
			view.Show();
		}

		// This method will be called when the CustomerDetail window is closed
		//private void CustomerDetailWindow_Closed(object sender, EventArgs e)
		//{
		//	if (sender is CustomerDetail customerDetailWindow)
		//	{
		//		customerDetailWindow.Closed -= CustomerDetailWindow_Closed;
		//	}

		//	FillData();
		//}

		private async void btnSave_Click(object sender, RoutedEventArgs e)
		{
			//int totalPrice = 0;

			//if (purchaseOrderDto == null)
			//{
			//    purchaseOrderDto = new PurchaseOrderDto();

			//    purchaseOrderDto.Status = "Pending";
			//    purchaseOrderDto.CreatedDate = DateTime.UtcNow.ToString();
			//    purchaseOrderDto.Type = "Purchase Order";
			//    purchaseOrderDto.TotalPrice = totalPrice;
			//    purchaseOrderDto.OrderDetails = new List<OrderDetail>();
			//    purchaseOrderDto.PaymentMethod = "cash";
			//    //purchaseOrderDto.UserId = int.TryParse(txtUser.Text, out var userId) ? userId : 0;
			//    purchaseOrderDto.UserId = _userSessionService.CurrentUser.Id;   
			//    //purchaseOrderDto.CustomerId = int.TryParse(txtCustomer.Text, out var customerId) ? customerId : 0;

			//    if (_purchaseOrderService.AddPurchaseOrder(purchaseOrderDto))
			//    {
			//        MessageBox.Show("Successfully added a new purchase order.", "Success",
			//                        MessageBoxButton.OK, MessageBoxImage.Information);
			//        btnSave.IsEnabled = false;  // Disable the Save button
			//        Close();
			//    }
			//}
			//else
			//{
			//    btnSave.IsEnabled = false;
			//}
		}

		//private async void btnDelete_Click(object sender, RoutedEventArgs e)
		//{
		//    if (_purchaseOrderService.DeletePurchaseOrder(purchaseOrderDto))
		//    {
		//        MessageBox.Show("Successfully deleted purchase order.", "Success",
		//                        MessageBoxButton.OK, MessageBoxImage.Information);
		//        Close();
		//    }
		//}
	}
}
