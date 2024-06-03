using Microsoft.IdentityModel.Tokens;
using Services;
using Services.Dto;
using System.Windows.Controls;

namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for ProductsListUI.xaml
    /// </summary>
    public partial class ProductsListUI : Page
    {
        ProductService _productService;
        public ProductsListUI(ProductService productService)
        {
            _productService = productService;
            InitializeComponent();
            LoadProductsData();
        }
        private async void LoadProductsData()
        {
            IEnumerable<ProductDto> products = await _productService.GetProducts();
            grdProductsList.ItemsSource = products;
        }

        private async void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IEnumerable<ProductDto> products = await _productService.GetProducts();
            grdProductsList.ItemsSource = products;
        }

        private async void btnSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var searchValue = txtSearch.Text;
            if (!searchValue.IsNullOrEmpty())
            {
                IEnumerable<ProductDto> products = await _productService.GeProductByName(searchValue);
                grdProductsList.ItemsSource = products;
            }                           
        }
    }
}
