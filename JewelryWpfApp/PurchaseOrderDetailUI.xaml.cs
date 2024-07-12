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
using Microsoft.IdentityModel.Tokens;

namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for PurchaseOrderDetailUI.xaml
    /// </summary>
    public partial class PurchaseOrderDetailUI : Window
    {
        private readonly ProductService _productService;
        private readonly GoldService _goldService;
        private readonly OrderDetailService _orderDetailService;
        private readonly PurchaseOrderService _purchaseOrderService;
        private readonly UserSessionService _userSessionService;
        private readonly CustomerService _customerService;
        private ProductDto? _selectedProduct = null;
        internal PurchaseOrderDto? purchaseOrderDto;

        public PurchaseOrderDetailUI(ProductService productService
            , GoldService goldService
            , OrderDetailService orderDetailService
            , PurchaseOrderService purchaseOrderService
            , UserSessionService userSessionService
            , CustomerService customerService)
        {
            _productService = productService;
            _goldService = goldService;
            _orderDetailService = orderDetailService;
            _purchaseOrderService = purchaseOrderService;
            _userSessionService = userSessionService;
            _customerService = customerService;
            InitializeComponent();
        }

        private async Task FillData()
        {
            int orderId = 0;
            float totalPrice = 0;
            if (purchaseOrderDto != null)
            {
                orderId = purchaseOrderDto.Id;

                IEnumerable<int> productIds = await _orderDetailService.GetPurchaseOrdersInOrderDetail(orderId);

                List<ProductDto> products = new List<ProductDto>();

                foreach (int productId in productIds)
                {
                    var product = _productService.GetProductById(productId);

                    products.Add(product);

                    totalPrice += (float)product.ProductPrice;
                }

                //txtCustomer.Text = purchaseOrderDto.CustomerId.ToString();

                txtTotalPrice.Text = totalPrice.ToString();

                dgvPurchaseOrder_ProductsList.ItemsSource = products;
            }
        }

        private async void PageLoaded(object sender, RoutedEventArgs e)
        {
            await FillData();
            SetupButton();
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            PurchaseOrderDetail_ProductDetail productDetailUI 
                = new PurchaseOrderDetail_ProductDetail(
                    _productService, _goldService, _orderDetailService);
            productDetailUI.OrderId = purchaseOrderDto.Id;
            productDetailUI.ShowDialog();
            await FillData();
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int totalPrice = 0;

            if (purchaseOrderDto == null)
            {
                purchaseOrderDto = new PurchaseOrderDto();

                purchaseOrderDto.Status = "Pending";
                purchaseOrderDto.CreatedDate = DateTime.UtcNow.ToString();
                purchaseOrderDto.Type = "Purchase Order";
                purchaseOrderDto.TotalPrice = totalPrice;
                purchaseOrderDto.OrderDetails = new List<OrderDetail>();
                purchaseOrderDto.PaymentMethod = "cash";
                purchaseOrderDto.UserId = _userSessionService.CurrentUser.Id;
                //purchaseOrderDto.CustomerId = int.TryParse(txtCustomer.Text, out var customerId) ? customerId : 0;

                if (!txtCustomerPhone.Text.IsNullOrEmpty())
                {
                    Customer? customer;
                    customer = await _customerService.searchCustomerByPhoneNumber(txtCustomerPhone.Text);

                    if (customer != null)
                    {
                        txtCustomerAddress.Text = customer.Address;
                        txtCustomerName.Text = customer.Name;
                        purchaseOrderDto.CustomerId = customer.Id;
                    }
                }
                Customer cus = new Customer();
                cus.Name = txtCustomerName.Text;
                cus.Address = txtCustomerAddress.Text;
                cus.Phone = txtCustomerPhone.Text;

                _customerService.AddCustomer(cus);

                purchaseOrderDto.CustomerId = cus.Id;

                if (_purchaseOrderService.AddPurchaseOrder(purchaseOrderDto))
                {
                    MessageBox.Show("Successfully added a new purchase order.", "Success",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                    btnSave.IsEnabled = false;  // Disable the Save button
                    Close();
                }
            }
            else
            {
                btnSave.IsEnabled = false;
            }
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var customer = await _customerService.searchCustomerByPhoneNumber(txtCustomerPhone.Text);

            if(customer != null)
            {
                txtCustomerAddress.Text = customer.Address;
                txtCustomerName.Text = customer.Name;
                txtCustomerPhone.Text = customer.Phone;
            }
            else
            {
               txtCustomerAddress.Text = null;
               txtCustomerName.Text = null;
               MessageBox.Show("This phone number does not exist!", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_purchaseOrderService.DeletePurchaseOrder(purchaseOrderDto))
            {
                MessageBox.Show("Successfully deleted purchase order.", "Success",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }

        private async void btnPaid_Click(object sender, RoutedEventArgs e)
        {
            if (_purchaseOrderService.IsPaid(purchaseOrderDto))
            {
                MessageBox.Show("Payment Successful!!!", "Success",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }

        private async void dgvProductsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvPurchaseOrder_ProductsList.SelectedItems.Count > 0)
            {
                try
                {
                    _selectedProduct = (ProductDto)dgvPurchaseOrder_ProductsList.SelectedItems[0];
                    PurchaseOrderDetail_ProductDetail productDetailUI =
                        new PurchaseOrderDetail_ProductDetail(_productService, _goldService, _orderDetailService);
                    productDetailUI.ProductDto = _selectedProduct;

                    productDetailUI.OrderId = purchaseOrderDto.Id;

                    productDetailUI.ShowDialog();

                    await FillData();
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
                _selectedProduct = null;
            }
        }

        private void SetupButton()
        {
            if (purchaseOrderDto != null && purchaseOrderDto.Status != "Pending")
            {
                btnSave.IsEnabled = false;
                btnDelete.IsEnabled = false;
                btnPaid.IsEnabled = false;
                btnAdd.IsEnabled = false;

                btnSave.Foreground = new SolidColorBrush(Colors.Black);
                btnDelete.Foreground = new SolidColorBrush(Colors.Black);
                btnPaid.Foreground = new SolidColorBrush(Colors.Black);
                btnAdd.Foreground = new SolidColorBrush(Colors.Black) ;
            }
            else if (purchaseOrderDto == null)
            {
                btnDelete.IsEnabled = false;
                btnPaid.IsEnabled = false;
                btnDelete.Foreground = new SolidColorBrush(Colors.Black);
                btnPaid.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

    }
}
 