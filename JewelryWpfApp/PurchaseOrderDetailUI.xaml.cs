using Services.Dto;
using Services;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;

namespace JewelryWpfApp
{
    /// <summary>
    /// Interaction logic for PurchaseOrderDetail_ProductDetail.xaml
    /// </summary>
    public partial class PurchaseOrderDetailUI : Window
    {
        private readonly GoldService _goldService;
        private readonly OrderDetailService _orderDetailService;

        public PurchaseOrderDetailDto? SelectedOrderDetail;
        public int OrderId; 
        public PurchaseOrderDetailUI(GoldService goldService, OrderDetailService orderDetailService)
        {
            _goldService = goldService;
            _orderDetailService = orderDetailService;
            InitializeComponent();
        }
        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = SelectedOrderDetail;
            FillData();
        }
        private async void FillData()
        {
            // fill gold type list
            cbGoldType.ItemsSource = await _goldService.GetAllGoldTypeAsync();
            cbGoldType.DisplayMemberPath = "Name";
            cbGoldType.SelectedValuePath = "Id";
            cbGoldType.SelectedIndex = 0;

            if (SelectedOrderDetail != null)
            {
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

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
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
                              (string.IsNullOrEmpty(txtGemWeight.Text) ? 0 : decimal.Parse(txtGemWeight.Text)),
                ImgUrl = selectedImg.Source == null ? "" : ((BitmapImage)selectedImg.Source).UriSource.ToString()
            };

            if (await _orderDetailService.AddOrderDetail(productDto, OrderId))
            {
                MessageBox.Show("Successfully added an item to order", "Success!!!",
                                 MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                MessageBox.Show("Fail to add an item to order", "Error!!!",
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
            var productDto = (ProductDto)DataContext;

            //if (await _orderDetailService.UpdatePurchaseOrderDetail(productDto, OrderId))
            //{
            //    MessageBox.Show("Successfully updated order item.",
            //                                      "Success",
            //                                      MessageBoxButton.OK,
            //                                      MessageBoxImage.Information);
            //    Close();
            //}
            //else
            //{
            //    MessageBox.Show("Error while updating an order item!",
            //                                                      "Error",
            //                                                      MessageBoxButton.OK,
            //                                                      MessageBoxImage.Error);
            //}
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you really want to delete this item!", "Delete confirm",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            //if (_productService.DeleteProduct(ProductDto))
            //{
            //    MessageBox.Show("Successfully deleted!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            //    Close();
            //}
            //else
            //{
            //    MessageBox.Show("Error while updating a product!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
