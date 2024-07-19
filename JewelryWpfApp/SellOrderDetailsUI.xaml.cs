using Services.Dto;
using System.Windows;

namespace JewelryWpfApp
{
	/// <summary>
	/// Interaction logic for SellOrdersUI.xaml
	/// </summary>
	public partial class SellOrderDetailsUI : Window
	{
		//private Order _sellOrder;
		public List<SellOrderDetailDto> Details;

		public SellOrderDetailsUI(List<SellOrderDetailDto> details)
		{
			Details = details;
			InitializeComponent();
		}

		private void PageLoaded(object sender, RoutedEventArgs e)
		{
			//var orders = GetOrders(); 
			dgSellOrders.ItemsSource = Details;
		}



	}
}
