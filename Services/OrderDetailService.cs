
using AutoMapper;
using Repositories.Entities;
using Repositories;
using Repositories.Entities;
using Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Helpers;
using System.Linq.Expressions;

namespace Services
{
    public class OrderDetailService
    {
        private readonly OrderDetailRepository _orderRepo;
        private readonly GoldPriceRepository _goldPriceRepository;
        private readonly OrderDetailRepository _orderDetailRepository;
        private readonly IMapper _mapper;

        public OrderDetailService(OrderDetailRepository orderRepo, IMapper mapper,
            GoldPriceRepository goldPriceRepository,
            OrderDetailRepository orderDetailRepository)
        {
            _orderRepo = orderRepo;
            _goldPriceRepository = goldPriceRepository;

            _mapper = mapper;
            _orderDetailRepository = orderDetailRepository;
        }
        public List<OrderDetail> GetAll(Expression<Func<OrderDetail, bool>>? filter = null, string? includeProperties = null) { 
            return _orderRepo.GetAll().ToList();
		}
        public OrderDetail Get(Expression<Func<OrderDetail, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            return _orderRepo.Get(filter, includeProperties, tracked);
		}

        public List<SellOrderDetailDto> GetDetailsFromOrder(int id)
        {
            if (id == 0) return null;
            var details = _orderRepo.GetDetailsFromOrder(id);
            List<SellOrderDetailDto> detailDtos = _mapper.Map<List<SellOrderDetailDto>>(details);
            foreach (var p in detailDtos)
            {
                p.GoldPrice = _goldPriceRepository.GetLatestGoldPrice(p.GoldId).BidPrice;
            }
            return detailDtos;
        }

		//public List<SellOrderDetailDto> Mapping(List<OrderDetail> details)
		//{
		//	if (id == 0) return null;
		//	var details = _orderRepo.GetDetailsFromOrder(id);
		//	List<SellOrderDetailDto> detailDtos = _mapper.Map<List<SellOrderDetailDto>>(details);
		//	foreach (var p in detailDtos)
		//	{
		//		p.GoldPrice = _goldPriceRepository.GetLatestGoldPrice(p.GoldId).BidPrice;
		//	}
		//	return detailDtos;
		//}

		public async Task<IEnumerable<int>> GetPurchaseOrdersInOrderDetail(int orderId) { 

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


        public void Save()
        {
            _orderRepo.Save();
        }

        public void Update(OrderDetail obj)
        {
            _orderRepo.Update(obj);
        }
    }
}
