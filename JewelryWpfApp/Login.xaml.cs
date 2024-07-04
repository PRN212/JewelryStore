using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Entities;
using Services;
using System.Windows;
using System.Windows.Controls;



namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        UserService _userService;
        private readonly IServiceProvider _serviceProvider;
        public Login(UserService userService, IServiceProvider serviceProvider)
        {
            _userService = userService;
            _serviceProvider = serviceProvider;
            InitializeComponent();          
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Password))
            {
                MessageBox.Show("Please input both email and password!");
                return;
            }

            User? acc = _userService.CheckLogin(txtUsername.Text, txtPassword.Password);
            // login fail
            if (acc == null)
            {
                MessageBox.Show("Login fail. Check the email and password again!");
                return;
            }

            // role manager
            if (acc.Role == "Manager")
            {
                var mainWindow = _serviceProvider.GetRequiredService<ManagerMainUI>();
                mainWindow.Show();
                Close();
            }

            // role staff
            if (acc.Role == "Staff")
            {
                var mainWindow = _serviceProvider.GetRequiredService<StaffMainUI>();
                mainWindow.Show();
                Close();
            }
                  
        }
    }
}
