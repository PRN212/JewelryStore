using Microsoft.Extensions.DependencyInjection;
using Repositories.Entities;
using Repositories.Entities.Orders;
using Services;
using Services.Dto;
using System.Windows;

namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for CustomerUI.xaml
    /// </summary>
    public partial class CustomerUI : Window
    {
        private readonly CustomerService _customerService;
        private readonly PurchaseOrderService _purchaseOrderService;
        private readonly UserSessionService _userSessionService;
        private readonly IServiceProvider _serviceProvider;

        private PurchaseOrderDto? _purchaseOrderDto;
        public CustomerUI(CustomerService customerService, PurchaseOrderService purchaseOrderService, UserSessionService userSessionService, IServiceProvider serviceProvider)
        {
            _customerService = customerService;
            _purchaseOrderService = purchaseOrderService;
            _userSessionService = userSessionService;
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var phone = txtPhone.Text;
            var customer = await _customerService.searchCustomerByPhoneNumber(phone);
            if (customer == null)
            {
                MessageBox.Show("This phone number does not exist!", "Warning!!!",
                                 MessageBoxButton.OK, MessageBoxImage.Exclamation);
                DataContext = null;   
            }
            else
            {
                DataContext = customer;
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // validate input
            if (string.IsNullOrEmpty(txtAddress.Text) || string.IsNullOrEmpty(txtPhone.Text) ||
                string.IsNullOrEmpty(txtName.Text)) 
            {
                MessageBox.Show("Please fill all necessary in formation!", "Warning!!!",
                                                 MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            Customer customer;
            if (DataContext == null)
            {
                customer = new Customer()
                {
                    Phone = txtPhone.Text,
                    Name = txtName.Text,
                    Address = txtAddress.Text,
                };
            }
            else
            {
                customer = (Customer) DataContext;
            }
            var userId = _userSessionService.CurrentUser.Id;
            _purchaseOrderDto = await _purchaseOrderService.AddOrder(customer, userId, OrderType.Purchase.ToString());
            if (_purchaseOrderDto == null)
            {
                MessageBox.Show("Something wrong while creating an order!", "Error!!!",
                                                 MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                // navigate to OrderUI
                var orderUI = _serviceProvider.GetRequiredService<PurchaseOrderUI>();
                orderUI.SelectedOrder = _purchaseOrderDto;
                orderUI.Show();
                Close();
            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
