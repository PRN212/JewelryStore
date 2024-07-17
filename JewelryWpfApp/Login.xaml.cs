using Microsoft.Extensions.DependencyInjection;
using Repositories.Entities;
using Services;
using System.Windows;

namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly UserService _userService;
        private readonly IServiceProvider _serviceProvider;
        private readonly UserSessionService _userSessionService;
        public Login(UserService userService, IServiceProvider serviceProvider, UserSessionService userSessionService)
        {
            _userService = userService;
            _serviceProvider = serviceProvider;
            _userSessionService = userSessionService;
            InitializeComponent();          
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Password))
            {
                MessageBox.Show("Please input both email and password!");
                return;
            }

            User? acc = await _userService.CheckLogin(txtUsername.Text, txtPassword.Password);
            // login fail
            if (acc == null)
            {
                MessageBox.Show("Login fail. Check the email and password again!");
                return;
            }

            // Store the logged-in user in the session service
            _userSessionService.CurrentUser = acc;

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
