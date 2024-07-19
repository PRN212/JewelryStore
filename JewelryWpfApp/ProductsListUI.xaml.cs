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
		private readonly GoldService _goldService;
		private ProductDto? _selected = null;

		public ProductsListUI(ProductService productService, GoldService goldService)
		{
			_productService = productService;
			_goldService = goldService;
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
				_selected = (ProductDto)dgvProductsList.SelectedItems[0];
				ProductDetail productDetailUI = new ProductDetail(_productService, _goldService);
				productDetailUI.ProductDto = _selected;
				productDetailUI.ShowDialog();

				await FillDataGridView();
			}
			else
			{
				_selected = null;
			}
		}

		private async void btnSearch_Click(object sender, RoutedEventArgs e)
		{
			var searchValue = txtSearch.Text;

			dgvProductsList.ItemsSource = await _productService.GeProductByName(searchValue);
		}

		private async void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			ProductDetail productDetailUI = new ProductDetail(_productService, _goldService);
			productDetailUI.ShowDialog();
			await FillDataGridView();
		}
	}
}
