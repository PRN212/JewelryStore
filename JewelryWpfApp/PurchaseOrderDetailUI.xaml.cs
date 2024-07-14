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
using Microsoft.EntityFrameworkCore;
using Repositories;

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
        private DataContext _dataContext;
        public Customer Customer;

        public PurchaseOrderDetailUI(ProductService productService
            , GoldService goldService
            , OrderDetailService orderDetailService
            , PurchaseOrderService purchaseOrderService
            , UserSessionService userSessionService
            , CustomerService customerService,
                DataContext dataContext)
        {
            _productService = productService;
            _goldService = goldService;
            _orderDetailService = orderDetailService;
            _purchaseOrderService = purchaseOrderService;
            _userSessionService = userSessionService;
            _customerService = customerService;
            InitializeComponent();
            _dataContext = dataContext;
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

                    if(product != null)
                    {
                        products.Add(product);

                        totalPrice += (float)product.ProductPrice;
                    }
                }

                //txtCustomer.Text = purchaseOrderDto.CustomerId.ToString();

                txtTotalPrice.Text = totalPrice.ToString();

                txtCustomerAddress.Text = Customer.Address;
                txtCustomerName.Text = Customer.Name;
                txtCustomerPhone.Text = Customer.Phone;

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
            if (purchaseOrderDto != null)
            {
                productDetailUI.OrderId = purchaseOrderDto.Id;
                productDetailUI.ShowDialog();
                await FillData();
            }else
            {
                if (txtCustomerName.Text.IsNullOrEmpty()  ||
                    txtCustomerPhone.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("Must enter Customer Name or phone!", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }else
                {
                    PurchaseOrderDto purchaseOrderDto = new PurchaseOrderDto();
                    var customerPhone = _customerService.searchCustomerByPhoneNumber(txtCustomerPhone.Text);
                    if (customerPhone != null)
                    {
                        Customer customer = new Customer();
                        customer.Id = customerPhone.Id;
                        customer.Name = txtCustomerName.Text;
                        if (txtCustomerAddress.Text != null)
                        {
                            customer.Address = txtCustomerAddress.Text;
                        }
                    }
                    purchaseOrderDto.CustomerName = txtCustomerName.Text;
                    purchaseOrderDto.OrderDetails = new List<OrderDetail>();
                    productDetailUI.ShowDialog();
                }
            }
            
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            decimal totalPrice = 0;

            if (purchaseOrderDto == null)
            {
                purchaseOrderDto = new PurchaseOrderDto();

                purchaseOrderDto.Status = "Pending";
                purchaseOrderDto.CreatedDate = DateTime.UtcNow.ToString();
                purchaseOrderDto.Type = "Purchase Order";
                purchaseOrderDto.TotalPrice = totalPrice;
                purchaseOrderDto.OrderDetails = new List<OrderDetail>();
                purchaseOrderDto.PaymentMethod = "cash";
                purchaseOrderDto.UserName = _userSessionService.CurrentUser.Username;
                purchaseOrderDto.UserId = _userSessionService.CurrentUser.Id;

                if (!txtCustomerPhone.Text.IsNullOrEmpty())
                {
                    Customer? customer;
                    customer = await _customerService.searchCustomerByPhoneNumber(txtCustomerPhone.Text);

                    if (customer != null)
                    {
                        txtCustomerAddress.Text = customer.Address;
                        txtCustomerName.Text = customer.Name;
                        purchaseOrderDto.CustomerName = customer.Name;
                    }
                }
                Customer cus = new Customer();
                cus.Name = txtCustomerName.Text;
                cus.Address = txtCustomerAddress.Text;
                cus.Phone = txtCustomerPhone.Text;

                _customerService.AddCustomer(cus);

                purchaseOrderDto.CustomerName = cus.Name;
                purchaseOrderDto.CustomerId = cus.Id;

                if (_purchaseOrderService.AddPurchaseOrder(purchaseOrderDto))
                {
                    MessageBox.Show("Successfully added a new purchase order.", "Success",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                    btnSave.IsEnabled = false;  // Disable the Save button
                    Close();
                }
            }else
            {
                Order order = _purchaseOrderService.GetPurchaseOrdersByIdreturnOrder(purchaseOrderDto.Id);
                purchaseOrderDto.TotalPrice = Convert.ToDecimal(txtTotalPrice.Text);
                _dataContext.Entry(order).State = EntityState.Detached;
                //await _purchaseOrderService.UpdatePurchaseOrder(purchaseOrderDto);
                if (await _purchaseOrderService.UpdatePurchaseOrder(purchaseOrderDto))
                {
                    MessageBox.Show("Successfully updated a purchase order.", "Success",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
                Close();
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

        //private async void btnDelete_Click(object sender, RoutedEventArgs e)
        //{
        //    if (_purchaseOrderService.DeletePurchaseOrder(purchaseOrderDto))
        //    {
        //        MessageBox.Show("Successfully deleted purchase order.", "Success",
        //                        MessageBoxButton.OK, MessageBoxImage.Information);
        //        Close();
        //    }
        //}

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this purchase order?",
                                                       "Confirm Delete",
                                                       MessageBoxButton.OKCancel,
                                                       MessageBoxImage.Question);

            if (result == MessageBoxResult.OK)
            {
                if (_purchaseOrderService.DeletePurchaseOrder(purchaseOrderDto))
                {
                    MessageBox.Show("Successfully deleted purchase order.", "Success",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();  // Close the window or perform any other necessary actions after deletion
                }
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
 