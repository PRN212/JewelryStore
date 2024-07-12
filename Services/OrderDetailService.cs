
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Entities;
using Services.Dto;
using System.Linq.Expressions;

namespace Services
{
    public class OrderDetailService
    {
        private readonly OrderDetailRepository _orderRepo;
        private readonly GoldPriceRepository _goldPriceRepository;

        private readonly IMapper _mapper;

        public OrderDetailService(OrderDetailRepository orderRepo, IMapper mapper,
            GoldPriceRepository goldPriceRepository) 
        {
            _orderRepo = orderRepo;
            _goldPriceRepository = goldPriceRepository;

            _mapper = mapper;

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
            var details = _orderRepo.GetDetailsFromOrder(id);
            List<SellOrderDetailDto> detailDtos = _mapper.Map<List<SellOrderDetailDto>>(details);
            foreach (var p in detailDtos)
            {
                p.GoldPrice = _goldPriceRepository.GetLatestGoldPrice(p.GoldId).BidPrice;
            }
            return detailDtos;
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
