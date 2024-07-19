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
using Static;
using Repositories.Entities.Orders;

namespace JewelryWpfApp
{
	/// <summary>
	/// Interaction logic for PurchaseOrderDetailUI.xaml
	/// </summary>
	public partial class AddSellOrderDetailUI : Window
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly ProductService _productService;
		private readonly OrderDetailService _orderDetailService;
		private readonly SellOrderService _sellOrderService;
		private readonly CustomerService _customerService;
		private int orderId = 0;
		private Order order;
		public event EventHandler OrderSaved;

		public AddSellOrderDetailUI(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			_productService = _serviceProvider.GetRequiredService<ProductService>();
			_orderDetailService = _serviceProvider.GetRequiredService<OrderDetailService>();
			_sellOrderService = _serviceProvider.GetRequiredService<SellOrderService>();
			_customerService = _serviceProvider.GetRequiredService<CustomerService>();
			InitializeComponent();
		}

		private async void FillData()
		{
			cbCustomer.ItemsSource = await _customerService.GetAllCustomersAsync();
			cbCustomer.SelectedValuePath = "Id";

			cbProduct.ItemsSource = await _productService.GetProducts();
			cbProduct.SelectedValuePath = "Id";
			cbProduct.DisplayMemberPath = "Name";

			txtRate.Text = "1.3";

			if (orderId == 0)
			{
				txtStatus.Text = "Pending";
				return;
			}
			txtStatus.Text = order.Status;
			txtPaymentMethod.Text = order.PaymentMethod;
			var details = _orderDetailService.GetDetailsFromOrder(orderId);
			dgvSellOrder.ItemsSource = details;
		}

		private async void PageLoaded(object sender, RoutedEventArgs e)
		{
			FillData();
		}

		private async Task<decimal> GetOrderDetailTotalAsync(int productId, int quantity, decimal rate)
		{
			var service = _serviceProvider.GetRequiredService<ProductService>();
			ProductDto product = await service.GetProductById(productId);

			return product.ProductPrice * quantity * rate;
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
					if (decimal.TryParse(txtRate.Text, out decimal rate))
					{
						if (rate < 1)
						{
							MessageBox.Show("Please enter a rate of at least 1", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
							return;
						}
						decimal detailPrice = await GetOrderDetailTotalAsync(productId, quantity, rate);
						// If not found, create a new OrderDetail
						OrderDetail detail = new OrderDetail()
						{
							OrderId = orderId,
							ProductId = productId,
							Quantity = quantity,
							Price = detailPrice
						};
						order.OrderDetails.Add(detail);
						order.TotalPrice += detailPrice;
						_sellOrderService.Update(order);
						_sellOrderService.Save();
					}
					else
					{
						MessageBox.Show("Please enter valid rate.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
						return;
					}
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

		}

		private void SaveOrder()
		{
			if (orderId != 0)
			{
				order.PaymentMethod = txtPaymentMethod.Text.Trim();
				order.Status = txtStatus.Text.Trim();

				order.CustomerId = (int)cbCustomer.SelectedValue;
				_sellOrderService.Update(order);
				_sellOrderService.Save();

				//MessageBox.Show("Order Header Saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			else
			{
				var userService = _serviceProvider.GetRequiredService<UserSessionService>();
				order = new Order()
				{
					PaymentMethod = txtPaymentMethod.Text.Trim(),
					Status = txtStatus.Text.Trim(),
					CustomerId = (int)cbCustomer.SelectedValue,
					CreatedDate = DateTime.Now,
					Type = SD.TypeSell,
					UserId = userService.CurrentUser.Id
				};
				_sellOrderService.Add(order);
				_sellOrderService.Save();
				orderId = order.Id;
				MessageBox.Show("Order created successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

			}
			OrderSaved?.Invoke(this, EventArgs.Empty);
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
			if (orderId == 0)
			{
				MessageBox.Show("Please create the order first", "Failure", MessageBoxButton.OK, MessageBoxImage.Error); 
				return;
			}
			PrintService printService = new(_serviceProvider);
			printService.ExportOrderToCsv(order.Id);
			MessageBox.Show("Print Succesfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
		}


	}
}
