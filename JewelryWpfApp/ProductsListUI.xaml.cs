using Microsoft.IdentityModel.Tokens;
using Repositories.Entities;
using Services;
using Services.Dto;
using System.Windows;
using System.Windows.Controls;

namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for ProductsListUI.xaml
    /// </summary>
    public partial class ProductsListUI : Page
    {
        private readonly ProductService _productService;
        private ProductDto _selected;

        public ProductsListUI(ProductService productService)
        {
            _productService = productService;
            InitializeComponent();
        }

        private async void PageLoaded(object sender, RoutedEventArgs e)
        {
            await FillDataGridView();
        }

        private async Task FillDataGridView()
        {
            IEnumerable<ProductDto> products = await _productService.GetProducts();
            dgvProductsList.ItemsSource = products;
        }

        private async void dgvProductsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvProductsList.SelectedItems.Count > 0)
            {
                try
                {
                    _selected = (ProductDto) dgvProductsList.SelectedItems[0];
                    ProductDetail productDetailUI = new ProductDetail(_productService);
                    productDetailUI._productDto = _selected;
                    productDetailUI.ShowDialog();

                    await FillDataGridView();
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
                _selected = null;
            }
        }

        private async void btnSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var searchValue = txtSearch.Text;
            if (!searchValue.IsNullOrEmpty())
            {
                IEnumerable<ProductDto> products = await _productService.GeProductByName(searchValue);
                dgvProductsList.ItemsSource = products;
            }                           
        }

        private async void btnSearch_Click_1(object sender, RoutedEventArgs e)
        {
            var searchValue = txtSearch.Text;

            dgvProductsList.ItemsSource = await _productService.GeProductByName(searchValue);
        }
    }
}
