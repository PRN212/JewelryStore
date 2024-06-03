using AutoMapper;
using Repositories.Entities;
using Services;
using System.Windows;
using System.Windows.Controls;



namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        UserService _userService;
        ProductService _productService;
        public Login(UserService userService, ProductService productService)
        {
            _userService = userService;
            _productService = productService;
            InitializeComponent();          
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please input both email and password!");
                return;
            }

            User? acc = _userService.CheckLogin(txtUsername.Text, txtPassword.Text);
            // login fail
            if (acc == null)
            {
                MessageBox.Show("Login fail. Check the email and password again!");
                return;
            }
            NavigationService?.Navigate(new ProductsListUI(_productService));
        }
    }
}
