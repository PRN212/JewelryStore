using Services.Dto;
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
using Repositories.Entities;
using System.Collections.ObjectModel;
using DTOs;

namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for SellOrdersUI.xaml
    /// </summary>
    public partial class SellOrderDetailsUI : Page
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
