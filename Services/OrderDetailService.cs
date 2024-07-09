using AutoMapper;
using Repositories.Entities;
using Repositories;
using Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Helpers;

namespace Services
{
    public class OrderDetailService
    {
        private readonly IMapper _mapper;
        private readonly OrderDetailRepository _orderDetailRepository;
        public OrderDetailService(IMapper mapper, 
            OrderDetailRepository orderDetailRepository)
        {
            _mapper = mapper;
            _orderDetailRepository = orderDetailRepository;
        }
        public async Task<IEnumerable<int>> GetPurchaseOrdersInOrderDetail(int orderId)
        {
            var orders = await _orderDetailRepository.GetOrderDetails(orderId);

            List<int> productIds = new List<int>();

            foreach (var order in orders)
            {
                int pId = order.ProductId;
                productIds.Add(pId);
            }

            return productIds;
        }

        public async Task<OrderDetail> GetOrderDetailByProductId(int productId)
        {
            var orderDetail = _orderDetailRepository.GetOrderDetailByProductId(productId);

            return await orderDetail;
        }

        public bool AddOrderDetail(OrderDetailDto orderDetailDto)
        {
            var orderDetail = _mapper.Map<OrderDetail>(orderDetailDto);
            return _orderDetailRepository.AddOrderDetail(orderDetail);
        }
    }
}
