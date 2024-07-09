using AutoMapper;
using Repositories;
using Repositories.Entities;
using Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderDetail_ProductOrderDetailService
    {
        private readonly OrderDetail_ProductOrderDetailRepository _productOrderDetailRepository;
        private readonly IMapper _mapper;
        public OrderDetail_ProductOrderDetailService(
            OrderDetail_ProductOrderDetailRepository productOrderDetailRepository,
            IMapper mapper)
        {
            _productOrderDetailRepository = productOrderDetailRepository;
            _mapper = mapper;
        }

        public bool AddOrderDetail(OrderDetailDto orderDetailDto)
        {
            var orderDetail = _mapper.Map<OrderDetail>(orderDetailDto);
            return _productOrderDetailRepository.AddOrderDetail_ProductOrderDetail(orderDetail);
        }
    }
}
