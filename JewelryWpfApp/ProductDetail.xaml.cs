using Microsoft.Win32;
using Services;
using Services.Dto;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for ProductDetail.xaml
    /// </summary>
    public partial class ProductDetail : Window
    {
        private readonly ProductService _productService;
        private readonly GoldService _goldService;
        public ProductDto? ProductDto { get; set; } 
        public ProductDetail(ProductService productService, GoldService goldService)
        {
            _productService = productService;
            _goldService = goldService;           
            InitializeComponent();
        }
        private async void PageLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = ProductDto;
            FillData();
        }
        private async void FillData()
        {
            // fill gold type list
            cbGoldType.ItemsSource = await _goldService.GetAllGoldTypeAsync();
            cbGoldType.DisplayMemberPath = "Name";
            cbGoldType.SelectedValuePath = "Id"; 
            cbGoldType.SelectedIndex = 0;

            if (ProductDto != null)
            {
                if (!string.IsNullOrEmpty(ProductDto.ImgUrl))
                {
                    Uri resourceUri = new Uri(ProductDto.ImgUrl,UriKind.Relative);
                    selectedImg.Source = new BitmapImage(resourceUri);
                }

                btnAdd.IsEnabled = false;
                btnAdd.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                btnUpdate.IsEnabled = false;
                btnDelete.IsEnabled = false;
                btnDelete.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private async void btnAdd_Click (object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text)) 
            {
                MessageBox.Show("Please enter product's name!", "Warning!!!",
                                 MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            var productDto = new ProductToAddDto()
            {
                Name = txtName.Text,
                Description = txtDescription.Text,
                GoldId = (int) cbGoldType.SelectedValue,
                GoldWeight = string.IsNullOrEmpty(txtGoldWeight.Text) ? 0 : decimal.Parse(txtGoldWeight.Text),
                GemName = txtGemType.Text,
                GemWeight = string.IsNullOrEmpty(txtGemWeight.Text) ? 0 : decimal.Parse(txtGemWeight.Text),
                GemPrice = string.IsNullOrEmpty(txtGemPrice.Text) ? 0 : decimal.Parse(txtGemPrice.Text),
                Labour = string.IsNullOrEmpty(txtLabour.Text) ? 0 : decimal.Parse(txtLabour.Text),
                Quantity = string.IsNullOrEmpty(txtQuantity.Text) ? 0 : int.Parse(txtQuantity.Text),
                TotalWeight = (string.IsNullOrEmpty(txtGoldWeight.Text) ? 0 : decimal.Parse(txtGoldWeight.Text)) + 
                (string.IsNullOrEmpty(txtGoldWeight.Text) ? 0 :decimal.Parse(txtGemWeight.Text)),
                ImgUrl = selectedImg.Source == null ? "" : ((BitmapImage)selectedImg.Source).UriSource.ToString()
            };

            if (await _productService.AddProduct(productDto))
            {
                MessageBox.Show("Successfully added a new product.","Success",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            } else
            {
                MessageBox.Show("Error while adding a new product!","Error",
                                 MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }          

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Please enter product's name!", "Warning!!!",
                                 MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            var productDto = (ProductDto) DataContext;

            if (await _productService.UpdateProduct(productDto))
            {
                MessageBox.Show("Successfully updated product.",
                                                  "Success",
                                                  MessageBoxButton.OK,
                                                  MessageBoxImage.Information);
                Close();
            }
            else
            {
                MessageBox.Show("Error while updating a product!",
                                                                  "Error",
                                                                  MessageBoxButton.OK,
                                                                  MessageBoxImage.Error);
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you really want to delete this item!", "Delete confirm",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            if (await _productService.DeleteProduct(ProductDto))
            {
                MessageBox.Show("Successfully deleted!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                MessageBox.Show("Error while updating a product!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void chooseImg_Click(object sender, RoutedEventArgs e)
        {
            // Tạo một hộp thoại chọn file
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Thiết lập các thuộc tính cho hộp thoại chọn file
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg;*.gif)|*.png;*.jpg;*.jpeg;*.gif|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures); 
            openFileDialog.Title = "Chọn ảnh";

            // Mở hộp thoại và xác nhận rằng người dùng đã chọn một file
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                try
                {
                    // Lấy đường dẫn tuong doi đến file đã chọn
                    string projectRoot = Directory.GetCurrentDirectory();
                    Uri fileUri = new Uri(openFileDialog.FileName);
                    Uri relativeUri = new Uri(projectRoot+"/",UriKind.Absolute).MakeRelativeUri(fileUri);
                    string relativePath = relativeUri.ToString();

                    // Hiển thị ảnh lên Image control
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(relativePath,UriKind.Relative);
                    bitmap.EndInit();

                    selectedImg.Source = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi khi mở và hiển thị ảnh: {ex.Message}");
                }
            }
        }
    }
}
