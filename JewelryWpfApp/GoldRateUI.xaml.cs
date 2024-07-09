using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Repositories.Entities;
using Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Xml;
using System.Xml.Serialization;

namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for GoldRateUI.xaml
    /// </summary>
    public partial class GoldRateUI : Page
    {
        private readonly GoldPriceService _goldPriceService;
        public GoldRateUI(GoldPriceService goldPriceService)
        {
            _goldPriceService = goldPriceService;
            InitializeComponent();
        }

        private GoldPriceFromAPI _selected = null;
        List<GoldPriceFromAPI> goldPriceData = new List<GoldPriceFromAPI>();
        DateTime dateTime;


        private void btnGetPrice_Click(object sender, RoutedEventArgs e)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;

            try
            {
                // Fetch dữ liệu từ API
                string apiUrl = "https://sjc.com.vn/xml/tygiavang.xml";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string xml = reader.ReadToEnd();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                // Lấy từng trường dữ liệu 
                string getUpdatedTime = doc.SelectSingleNode("/root/ratelist").Attributes["updated"].InnerText;
                string[] splitTime = getUpdatedTime.Split(' ');
                string updatedTime = splitTime[2] + " " + splitTime[0];
                dateTime = DateTime.ParseExact(updatedTime, "dd/MM/yyyy HH:mm:ss", provider);

                lblUpdateTime.Content = $"Update time: {dateTime}";

                var nodes = doc.SelectNodes("/root/ratelist/city");
                var childNote = nodes[0];
                int index = 1;
                foreach (XmlNode node in childNote)
                {
                    GoldPriceFromAPI data = new GoldPriceFromAPI();
                    data.GoldId = index;
                    data.GoldName = node.Attributes["type"].InnerText;
                    data.BuyingPrice = decimal.Parse(node.Attributes["buy"].InnerText) * 1000;
                    data.SellingPrice = decimal.Parse(node.Attributes["sell"].InnerText) * 1000;
                    data.BuyingRate = data.BuyingPrice;
                    data.SellingRate = data.SellingPrice;
                    goldPriceData.Add(data);
                    index++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading gold price data: {ex.Message}");
                return;
            }

            grdGoldRate.ItemsSource = goldPriceData;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_selected != null)
            {
                if (string.IsNullOrEmpty(tbChargeRate.Text))
                {
                    MessageBox.Show("Please fill in all the required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (decimal.TryParse(tbChargeRate.Text, out decimal chargeRate) && chargeRate >=0)
                {
                    //foreach (GoldPriceFromAPI gold in goldPriceData)
                    //{
                    //    if (gold.GoldId.Equals(_selected.GoldId))
                    //    {
                    //        gold.BuyingRate = gold.BuyingPrice * chargeRate;
                    //        gold.SellingRate = gold.SellingPrice * chargeRate;
                    //        ReloadDataGrid();
                    //    }
                    //}
                    _selected.BuyingRate = _selected.BuyingPrice * chargeRate;
                    _selected.SellingRate = _selected.SellingPrice * chargeRate;
                    ReloadDataGrid();
                }
                else
                {
                    MessageBox.Show("Please enter valid numeric values.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("You must choose an item to Update!", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_selected != null)
            {
                GoldPrice goldPrice = new GoldPrice()
                {
                    GoldId = _selected.GoldId,
                    DateTime = dateTime,
                    AskPrice = _selected.BuyingRate,
                    BidPrice = _selected.SellingRate
                };
                _goldPriceService.SaveNewGoldPrice(goldPrice);
                MessageBox.Show("Save successfully!", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("You must choose an item to Update!", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void grdGoldRate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdGoldRate.SelectedItems.Count > 0 ) 
            {
                _selected = (GoldPriceFromAPI)grdGoldRate.SelectedItems[0];
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ReloadDataGrid()
        {
            grdGoldRate.ItemsSource = null;
            grdGoldRate.ItemsSource = goldPriceData;
        }

        private void btnSaveAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (GoldPriceFromAPI gold in goldPriceData)
            {
                GoldPrice goldPrice = new GoldPrice()
                {
                    GoldId = gold.GoldId,
                    DateTime = dateTime,
                    AskPrice = gold.BuyingRate,
                    BidPrice = gold.SellingRate
                };
                _goldPriceService.SaveNewGoldPrice(goldPrice);
            }
            MessageBox.Show("Save successfully!", "Save All", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private class GoldPriceFromAPI
        {
            public int GoldId { get; set; }
            public string GoldName { get; set; }
            public decimal BuyingPrice { get; set; }
            public decimal SellingPrice { get; set; }
            public decimal BuyingRate { get; set; }
            public decimal SellingRate { get; set; }
        }
    }

    public class DecimalToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal)
            {
                return ((decimal)value).ToString("0");
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                if (decimal.TryParse((string)value, out decimal result))
                {
                    return result;
                }
            }
            return value;
        }
    }
}
