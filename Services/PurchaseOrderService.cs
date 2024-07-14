using AutoMapper;
using Repositories.Entities;
using Repositories;
using Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PurchaseOrderService
    {
        private readonly IMapper _mapper;
        private readonly ProductRepository _productRepository;
        private readonly GoldPriceRepository _goldPriceRepository;
        private readonly OrderRepository _orderRepository;
        public PurchaseOrderService(IMapper mapper, ProductRepository productRepository,
            GoldPriceRepository goldPriceRepository, OrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _goldPriceRepository = goldPriceRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PurchaseOrderDto>> GetPurchaseOrders()
        {
            var orders = await _orderRepository.GetPurchaseOrders();
            var purchaseOrdersDto = _mapper.Map<IEnumerable<Order>, IEnumerable<PurchaseOrderDto>>(orders);

            return purchaseOrdersDto;
        }

        public async Task<IEnumerable<PurchaseOrderDto>> GetPurchaseOrdersByDate(string day)
        {
            if (DateTime.TryParse(day, out DateTime searchDate))
            {
                var orders = await _orderRepository.SearchPurchaseOrdersByDate(searchDate);
                var purchaseOrdersDto = _mapper.Map<IEnumerable<Order>, IEnumerable<PurchaseOrderDto>>(orders);

                return purchaseOrdersDto;
            }
            else
            {
                // Nếu ngày không hợp lệ, trả về danh sách rỗng hoặc xử lý lỗi tùy theo yêu cầu.
                return Enumerable.Empty<PurchaseOrderDto>();
            }
        }

        public PurchaseOrderDto GetPurchaseOrdersById(int orderId)
        {
            var orders = _orderRepository.GetPurchaseOrderById(orderId);
            var purchaseOrdersDto = _mapper.Map<Order, PurchaseOrderDto>(orders);

            return purchaseOrdersDto;
        }

        public Order GetPurchaseOrdersByIdreturnOrder(int orderId)
        {
            var order = _orderRepository.GetPurchaseOrderById(orderId);

            return order;
        }

        public bool AddPurchaseOrder(PurchaseOrderDto purchaseOrderDto)
        {
            var purchaseOrder = _mapper.Map<Order>(purchaseOrderDto);
            return _orderRepository.AddPurchaseOrder(purchaseOrder);
        }

        public Task<bool> UpdatePurchaseOrder(PurchaseOrderDto purchaseOrderDto)
        {
            var purchaseOrder = _mapper.Map<Order>(purchaseOrderDto);
            return _orderRepository.UpdatePurchaseOrder(purchaseOrder);
        }

        public bool DeletePurchaseOrder(PurchaseOrderDto purchaseOrderDto)
        {
            var purchaseOrder = _mapper.Map<Order>(purchaseOrderDto);
            return _orderRepository.DeletePurchaseOrder(purchaseOrder);
        }

        public bool IsPaid(PurchaseOrderDto purchaseOrderDto)
        {
            var purchaseOrder = _mapper.Map<Order>(purchaseOrderDto);
            return _orderRepository.IsPaid(purchaseOrder);
        }

        public bool UpdateProduct(ProductDto productDto)
        {
            Product? product = _productRepository.GetProductById(productDto.Id);
            if (product == null) { return false; }
            _mapper.Map(productDto, product);
            return _productRepository.UpdateProduct(product);
        }

        public async Task<IEnumerable<Product>> SearchProducts(string searchValue)
        {
            return await _productRepository.SearchProducts(searchValue);
        }
    }
}
