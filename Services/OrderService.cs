
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Entities;
using Services.Dto;
using Static;
using System.Linq.Expressions;

namespace Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepo;
        private readonly IMapper _mapper;
        public OrderService(OrderRepository orderRepo, IMapper mapper) 
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
        }
        public List<Order> GetAll(Expression<Func<Order, bool>>? filter = null, string? includeProperties = null) { 
            return _orderRepo.GetAll().ToList();
		}

        public List<SellOrderDto> GetSellOrders()
        {
            // TODO: includeProp for Details
            List<Order> orders = _orderRepo.GetAll(o => o.Type == SD.TypeSell, includeProperties: "User,Customer").ToList();
            List<SellOrderDto> orderDtos = _mapper.Map<List<SellOrderDto>>(orders);

            return orderDtos;
        }
        public Order Get(Expression<Func<Order, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            return _orderRepo.Get(filter, includeProperties, tracked);
		}

		public void Save()
		{
			_orderRepo.Save();
		}

		public void Update(Order obj)
		{
			_orderRepo.Update(obj);
		}
	}
}
