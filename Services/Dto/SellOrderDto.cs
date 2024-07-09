using Repositories.Entities;
using Static;
using System.Collections.ObjectModel;

namespace DTOs
{
    public class SellOrderDto : PropChanged
    {
        private Order _order;
        public Order Order
        {
            get => _order;
            set
            {
                _order = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<OrderDetailDto> _orderDetails;
        public ObservableCollection<OrderDetailDto> OrderDetails
        {
            get => _orderDetails;
            set
            {
                _orderDetails = value;
                OnPropertyChanged();
            }
        }

        public SellOrderDto()
        {
            OrderDetails = new ObservableCollection<OrderDetailDto>();
        }
    }

    public class OrderDetailDto : PropChanged
    {
        private int _productId;
        public int ProductId
        {
            get => _productId;
            set
            {
                _productId = value;
                OnPropertyChanged();
            }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }

        private float _price;
        public float Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        // Assuming you might also want to include Product details in the DTO
        private string _productName;
        public string ProductName
        {
            get => _productName;
            set
            {
                _productName = value;
                OnPropertyChanged();
            }
        }
    }
}
