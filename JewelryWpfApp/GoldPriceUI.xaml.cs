using Repositories.Entities;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for GoldPriceUI.xaml
    /// </summary>
    public partial class GoldPriceUI : Page
    {
        private readonly GoldPriceService _goldPriceService;
        private readonly GoldService _goldService;
        public GoldPriceUI(GoldPriceService goldPriceService, GoldService goldService)
        {
            _goldPriceService = goldPriceService;
            InitializeComponent();
            _goldService = goldService;
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            //List<GoldPrice> goldPrices = _goldPriceService.GetLatestGoldPrices();
            //grdGoldPrice.ItemsSource = null;
            //grdGoldPrice.ItemsSource = goldPrices;

            grdGoldPrice.ItemsSource = null;
            grdGoldPrice.ItemsSource = _goldService.GetAllGoldType();
        }

        private void grdGoldPrice_Loaded(object sender, RoutedEventArgs e)
        {
            //List<GoldPrice> goldPrices = _goldPriceService.GetLatestGoldPrices();
            //grdGoldPrice.ItemsSource = null;
            //grdGoldPrice.ItemsSource = goldPrices;

            grdGoldPrice.ItemsSource = null;
            grdGoldPrice.ItemsSource = _goldService.GetAllGoldType();
        }
    }
}
