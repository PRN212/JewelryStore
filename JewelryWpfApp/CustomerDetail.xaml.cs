using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Repositories.Entities;
using System.Windows;

namespace JewelryWpfApp
{
	/// <summary>
	/// Interaction logic for Customer.xaml
	/// </summary>
	public partial class CustomerDetail : Window
	{
		private IServiceProvider service;
		public event EventHandler CustomerSaved;
		public CustomerDetail(IServiceProvider serviceProvider)
		{
			InitializeComponent();
			service = serviceProvider;
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			// Create a new Customer object from the input fields
			var customer = new Customer
			{
				Name = NameTextBox.Text,
				Phone = PhoneTextBox.Text,
				Address = AddressTextBox.Text
			};
			var customerRepo = service.GetRequiredService<CustomerRepository>();
			if (!customerRepo.AddCustomer(customer))
			{
				MessageBox.Show(GetWindow(this), "Failed to add customer", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				MessageBox.Show(GetWindow(this), "Customer added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

				// Raise the event
				CustomerSaved?.Invoke(this, EventArgs.Empty);
			}

		}
	}
}
