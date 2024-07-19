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
using Repositories.Entities.Orders;

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
			//txtRate.Text = "1.3";
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

		private async Task<decimal> GetOrderDetailTotalAsync(int productId, int quantity)
		{
			var service = _serviceProvider.GetRequiredService<ProductService>();
			ProductDto product = await service.GetProductById(productId);

			return product.ProductPrice * quantity;
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

				decimal detailPrice = await GetOrderDetailTotalAsync(productId, quantity);
				if (existingDetail != null)
				{
					// If found, only update the quantity
					existingDetail.Quantity += quantity;

				}
				else
				{
					//if (decimal.TryParse(txtRate.Text, out decimal rate))
					//{
					//	if (rate < 1)
					//	{
					//		MessageBox.Show("Please enter a rate of at least 1", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					//		return;
					//	}
					// If not found, create a new OrderDetail
					OrderDetail detail = new OrderDetail()
					{
						OrderId = orderId,
						ProductId = productId,
						Quantity = quantity,
						Price = detailPrice
					};
					order.OrderDetails.Add(detail);
					//}
					//else
					//{
					//	MessageBox.Show("Please enter valid rate.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					//	return;
					//}
				}
				order.TotalPrice += detailPrice;
				txtSearch.Text = "0";
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


		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			if (!Validate()) return;
			SaveOrder();
			MessageBox.Show("Order Saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

		}

		private void SaveOrder()
		{

			order.PaymentMethod = txtPaymentMethod.Text.Trim();
			order.Status = txtStatus.Text.Trim();

			order.CustomerId = (int)cbCustomer.SelectedValue;
			_sellOrderService.Update(order);
			_sellOrderService.Save();
			OrderSaved?.Invoke(this, EventArgs.Empty);

			//MessageBox.Show("Order Header saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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

		private void btnPrint_Click(object sender, RoutedEventArgs e)
		{
			PrintService printService = new(_serviceProvider);
			printService.ExportOrderToCsv(order.Id);
			MessageBox.Show("Print Succesfully to desktop", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
		}


		private async void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			order.Status = "Canceled";
			MessageBox.Show("Successfully canceled.", "Success",
							MessageBoxButton.OK, MessageBoxImage.Information);
			OrderSaved?.Invoke(this, EventArgs.Empty);

			Close();
		}
		private async void btnPaid_Click(object sender, RoutedEventArgs e)
		{
			order.Status = "Paid";
			MessageBox.Show("Successfully paid", "Success",
							MessageBoxButton.OK, MessageBoxImage.Information);
			OrderSaved?.Invoke(this, EventArgs.Empty);

			//Close();
		}

		private async void btnSearchProduct_Click(object sender, RoutedEventArgs eventArgs)
		{
			if (int.TryParse(txtSearch.Text, out int pId))
			{
				var product = await _productService.GetProductById(pId);
				if (product == null)
				{
					MessageBox.Show("Product not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
				else
				{
					cbProduct.SelectedValue = product.Id;
				}
			}
			else
			{
				MessageBox.Show("Please enter a valid id to search.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
		}

		private void cbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cbProduct.SelectedItem is ProductDto selectedProduct)
			{
				txtSearch.Text = selectedProduct.Id.ToString();
			}
		}
	}
}
