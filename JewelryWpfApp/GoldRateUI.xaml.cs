using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
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
        public GoldRateUI()
        {
            InitializeComponent();
        }

        private void btnGetPrice_Click(object sender, RoutedEventArgs e)
        {
            //var API_URL = @"https://sjc.com.vn/xml/tygiavang.xml";
            //XmlDocument xml = new XmlDocument();
            //xml.LoadXml(API_URL);
            //var updated = DateTime.Parse(xml.SelectSingleNode("/root/ratelist").Attributes["updated"].InnerText).ToString("dd/MM/yyyy HH:mm:ss");
            //lblUpdateTime.Content = $"Update time: {updated}";

            //var listNode = xml.SelectNodes("/root/ratelist/city");
            //foreach (XmlNode node in listNode)
            //{
            //    var nameCity = node.Attributes["name"].InnerText;
            //    var childNodeItem = node.ChildNodes;
            //    if (childNodeItem.Count > 0)
            //    {
            //        foreach (XmlNode childNode in childNodeItem)
            //        {
            //            var buy = childNode.Attributes["buy"].InnerText;
            //            var sell = childNode.Attributes["sell"].InnerText;
            //            var type = childNode.Attributes["type"].InnerText;
            //            tableRate.Rows.Add(nameCity, type, double.Parse(buy), double.Parse(sell));
            //        }
            //    }
            //}

            List<GoldPriceData> goldPriceData = new List<GoldPriceData>();

            try
            {
                // Fetch the XML data from the API
                string apiUrl = "https://sjc.com.vn/xml/tygiavang.xml";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string xml = reader.ReadToEnd();

                // Load the XML data into an XmlDocument
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                var updated = doc.SelectSingleNode("/root/ratelist").Attributes["updated"].InnerText;
                lblUpdateTime.Content = $"Update time: {updated}";

                // Extract the data from the XmlDocument and populate the GoldPriceData list
                var nodes = doc.SelectNodes("/root/ratelist/city");
                var childNote = nodes[0];
                foreach (XmlNode node in childNote)
                {
                    GoldPriceData data = new GoldPriceData();
                    data.GoldName = node.Attributes["type"].InnerText;
                    data.BuyingPrice = node.Attributes["buy"].InnerText;
                    data.SellingPrice = node.Attributes["sell"].InnerText;
                    goldPriceData.Add(data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading gold price data: {ex.Message}");
                return;
            }

            // Bind the data to the DataGrid
            grdGoldRate.ItemsSource = goldPriceData;
        }

        public class GoldPriceData
        {
            public string GoldName { get; set; }
            public string BuyingPrice { get; set; }
            public string SellingPrice { get; set; }
        }
    }
}
