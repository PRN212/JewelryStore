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

namespace JewelryWpfApp
{
	/// <summary>
	/// Interaction logic for PurchaseOrderDetailUI.xaml
	/// </summary>
	public partial class UpsertSellOrderDetailUI : Window
	{
		private readonly ProductService _productService;
		private readonly OrderDetailService _orderDetailService;
		private readonly SellOrderService _sellOrderService;
		private int orderId = 0;

		public UpsertSellOrderDetailUI(ProductService productService
			, OrderDetailService orderDetailService
			, SellOrderService sellOrderService, int orderId)
		{
			_productService = productService;
			_orderDetailService = orderDetailService;
			_sellOrderService = sellOrderService;
			this.orderId = orderId;
			InitializeComponent();
		}

		private void FillData()
		{
			var details = _orderDetailService.GetDetailsFromOrder(orderId);
			if (details is not null)
				dgvSellOrder.ItemsSource = details;
		}

		private async void PageLoaded(object sender, RoutedEventArgs e)
		{
			FillData();
		}

		private async void btnAdd_Click(object sender, RoutedEventArgs e)
		{

			FillData();
		}

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
