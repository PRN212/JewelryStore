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
	public partial class UpdateSellOrderDetailUI : Window
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly ProductService _productService;
		private readonly OrderDetailService _orderDetailService;
		private readonly SellOrderService _sellOrderService;
		private readonly CustomerService _customerService;
		private int orderId = 0;
		private Order order;
		public event EventHandler OrderSaved;

		public UpdateSellOrderDetailUI(IServiceProvider serviceProvider, int orderId)
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

			txtPaymentMethod.Text = order.PaymentMethod;
			txtRate.Text = "1.3";
			txtStatus.Text = order.Status;

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
			if (!Validate()) return;
			SaveOrder();

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
			// Subscribe to the saving event
			view.CustomerSaved += CustomerDetail_CustomerSaved;
			view.Closed += (s, e) => view.CustomerSaved -= CustomerDetail_CustomerSaved;
			view.Show();
		}

		private void CustomerDetail_CustomerSaved(object sender, EventArgs e)
		{

			// Refresh data after the customer is saved
			FillData();
		}

		private float GetTotalPrice()
		{
			// TODO
			return 0;
		}


		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			if (!Validate()) return;
			SaveOrder();

		}

		private void SaveOrder()
		{

			order.PaymentMethod = txtPaymentMethod.Text.Trim();
			order.Status = txtStatus.Text.Trim();

			order.CustomerId = (int)cbCustomer.SelectedValue;
			_sellOrderService.Update(order);
			_sellOrderService.Save();
			OrderSaved?.Invoke(this, EventArgs.Empty);

			MessageBox.Show("Order saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private bool Validate()
		{
			if (string.IsNullOrEmpty(txtPaymentMethod.Text))
			{
				MessageBox.Show("Please enter a payment method.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			if (string.IsNullOrEmpty(txtStatus.Text))
			{
				MessageBox.Show("Please enter a status.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			if (cbCustomer.SelectedItem == null)
			{
				MessageBox.Show("Please select a customer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			return true;
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
