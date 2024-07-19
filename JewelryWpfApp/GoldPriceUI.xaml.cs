using Repositories.Entities;
using Services;
using System.Windows;
using System.Windows.Controls;

namespace JewelryWpfApp
{
	/// <summary>
	/// Interaction logic for GoldPriceUI.xaml
	/// </summary>
	public partial class GoldPriceUI : Page
	{
		private readonly GoldPriceService _goldPriceService;
		public GoldPriceUI(GoldPriceService goldPriceService)
		{
			_goldPriceService = goldPriceService;
			InitializeComponent();
		}

		private void btnReload_Click(object sender, RoutedEventArgs e)
		{
			List<GoldPrice> goldPrices = _goldPriceService.GetLatestGoldPrices();
			grdGoldPrice.ItemsSource = null;
			grdGoldPrice.ItemsSource = goldPrices;
		}

		private void grdGoldPrice_Loaded(object sender, RoutedEventArgs e)
		{
			List<GoldPrice> goldPrices = _goldPriceService.GetLatestGoldPrices();
			grdGoldPrice.ItemsSource = null;
			grdGoldPrice.ItemsSource = goldPrices;
		}
	}
}
