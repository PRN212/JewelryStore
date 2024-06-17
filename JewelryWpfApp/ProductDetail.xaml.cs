using Services;
using Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for ProductDetail.xaml
    /// </summary>
    public partial class ProductDetail : Window
    {
        private readonly ProductService _productService;
        public ProductDto _productDto;
        public ProductDetail(ProductService productService)
        {
            _productService = productService;
            InitializeComponent();
        }
        private async void PageLoaded(object sender, RoutedEventArgs e)
        {
            FillData();
        }
        private void FillData()
        {
            if (_productDto != null)
            {
                txtName.Text = _productDto.Name;
                txtDescription.Text = _productDto.Description;
                //cbGoldType.SelectedItem = _productDto.GoldName;
                txtGoldWeight.Text = _productDto.GoldWeight.ToString();
                txtGoldPrice.Text = _productDto.GoldPrice.ToString();
                txtGemType.Text = _productDto.GemName;
                txtGemWeight.Text = _productDto.GemWeight.ToString();
                txtGemPrice.Text = _productDto.GemPrice.ToString();
                txtLabour.Text = _productDto.Labour.ToString();
                txtProductPrice.Text = _productDto.ProductPrice.ToString();
                txtQuantity.Text = _productDto.Quantity.ToString();
            }

        }

        private void btnAdd_Click (object sender, RoutedEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
