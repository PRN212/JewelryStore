using Microsoft.Win32;
using Services;
using Services.Dto;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
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
		public ProductDto ProductDto;
		public ProductDetail(ProductService productService, GoldService goldService)
		{
			_productService = productService;
			_goldService = goldService;
			InitializeComponent();
		}
		private async void PageLoaded(object sender, RoutedEventArgs e)
		{
			FillData();
		}
		private async void FillData()
		{
			// fill gold type list
			cbGoldType.ItemsSource = await _goldService.GetAllGoldTypeAsync();
			cbGoldType.DisplayMemberPath = "Name";
			cbGoldType.SelectedValuePath = "Id";

			if (ProductDto != null)
			{
				txtName.Text = ProductDto.Name;
				txtDescription.Text = ProductDto.Description;
				cbGoldType.SelectedValue = ProductDto.GoldId;
				txtGoldWeight.Text = ProductDto.GoldWeight.ToString();
				txtGoldPrice.Text = ProductDto.GoldPrice.ToString();
				txtGemType.Text = ProductDto.GemName;
				txtGemWeight.Text = ProductDto.GemWeight.ToString();
				txtGemPrice.Text = ProductDto.GemPrice.ToString();
				txtLabour.Text = ProductDto.Labour.ToString();
				txtProductPrice.Text = ProductDto.ProductPrice.ToString();
				txtQuantity.Text = ProductDto.Quantity.ToString();

				if (!string.IsNullOrEmpty(ProductDto.ImgUrl))
				{
					Uri resourceUri = new Uri(ProductDto.ImgUrl, UriKind.Relative);
					selectedImg.Source = new BitmapImage(resourceUri);
				}

				btnAdd.IsEnabled = false;
				btnAdd.Foreground = new SolidColorBrush(Colors.Black);
			}
			else
			{
				// set default value
				cbGoldType.SelectedIndex = 0;
				txtGoldWeight.Text = "0";
				txtGemWeight.Text = "0";
				txtGemPrice.Text = "0";
				txtLabour.Text = "0";
				txtQuantity.Text = "0";
				txtGoldPrice.Text = "0";
				txtProductPrice.Text = "0";

				btnUpdate.IsEnabled = false;
				btnDelete.IsEnabled = false;
				btnDelete.Foreground = new SolidColorBrush(Colors.Black);
			}
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
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
				GoldId = (int)cbGoldType.SelectedValue,
				GoldWeight = string.IsNullOrEmpty(txtGoldWeight.Text) ? 0 : decimal.Parse(txtGoldWeight.Text),
				GemName = txtGemType.Text,
				GemWeight = string.IsNullOrEmpty(txtGemWeight.Text) ? 0 : decimal.Parse(txtGemWeight.Text),
				GemPrice = string.IsNullOrEmpty(txtGemPrice.Text) ? 0 : decimal.Parse(txtGemPrice.Text),
				Labour = string.IsNullOrEmpty(txtLabour.Text) ? 0 : decimal.Parse(txtLabour.Text),
				Quantity = string.IsNullOrEmpty(txtQuantity.Text) ? 0 : int.Parse(txtQuantity.Text),
				TotalWeight = (string.IsNullOrEmpty(txtGoldWeight.Text) ? 0 : decimal.Parse(txtGoldWeight.Text)) +
				(string.IsNullOrEmpty(txtGoldWeight.Text) ? 0 : decimal.Parse(txtGemWeight.Text)),
				ImgUrl = selectedImg.Source == null ? "" : ((BitmapImage)selectedImg.Source).UriSource.ToString()
			};

			if (_productService.AddProduct(productDto))
			{
				MessageBox.Show("Successfully added a new product.", "Success",
								MessageBoxButton.OK, MessageBoxImage.Information);
				Close();
			}
			else
			{
				MessageBox.Show("Error while adding a new product!", "Error",
								 MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void btnUpdate_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(txtName.Text))
			{
				MessageBox.Show("Please enter product's name!", "Warning!!!",
								 MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}
			var productDto = new ProductDto()
			{
				Id = ProductDto.Id,
				Name = txtName.Text,
				Description = txtDescription.Text,
				GoldId = (int)cbGoldType.SelectedValue,
				GoldWeight = string.IsNullOrEmpty(txtGoldWeight.Text) ? 0 : decimal.Parse(txtGoldWeight.Text),
				GemName = txtGemType.Text,
				GemWeight = string.IsNullOrEmpty(txtGemWeight.Text) ? 0 : decimal.Parse(txtGemWeight.Text),
				GemPrice = string.IsNullOrEmpty(txtGemPrice.Text) ? 0 : decimal.Parse(txtGemPrice.Text),
				Labour = string.IsNullOrEmpty(txtLabour.Text) ? 0 : decimal.Parse(txtLabour.Text),
				Quantity = string.IsNullOrEmpty(txtQuantity.Text) ? 0 : int.Parse(txtQuantity.Text),
				TotalWeight = (string.IsNullOrEmpty(txtGoldWeight.Text) ? 0 : decimal.Parse(txtGoldWeight.Text)) +
				(string.IsNullOrEmpty(txtGoldWeight.Text) ? 0 : decimal.Parse(txtGemWeight.Text)),
				ImgUrl = selectedImg.Source == null ? "" : ((BitmapImage)selectedImg.Source).UriSource.ToString()
			};

			if (_productService.UpdateProduct(productDto))
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

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show("Do you really want to delete this item!", "Delete confirm",
				MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (result == MessageBoxResult.No)
			{
				return;
			}
			if (_productService.DeleteProduct(ProductDto))
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
					Uri relativeUri = new Uri(projectRoot + "/", UriKind.Absolute).MakeRelativeUri(fileUri);
					string relativePath = relativeUri.ToString();

					// Hiển thị ảnh lên Image control
					BitmapImage bitmap = new BitmapImage();
					bitmap.BeginInit();
					bitmap.UriSource = new Uri(relativePath, UriKind.Relative);
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
