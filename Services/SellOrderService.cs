
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Entities.Orders;
using Services.Dto;
using Static;
using System.Buffers;
using System.Linq.Expressions;

namespace Services
{
    public class SellOrderService
    {
        private readonly OrderRepository _orderRepo;
        //private readonly SellProductRepository _productRepository;
        //private readonly OrderDetailRepository _orderDetailRepository;
        private readonly IMapper _mapper;
        public SellOrderService(OrderRepository orderRepo, IMapper mapper
            //, SellProductRepository productRepository, OrderDetailRepository orderDetailRepository
            ) 
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
            //_productRepository = productRepository;
            //_orderDetailRepository = orderDetailRepository;
        }
        public List<Order> GetAll(Expression<Func<Order, bool>>? filter = null, string? includeProperties = null) { 
            return _orderRepo.GetAll(filter, includeProperties).ToList();
		}

        public List<SellOrderDto> GetSellOrders()
        {
            
            List<Order> orders = _orderRepo.GetAll(o => o.Type == SD.TypeSell, includeProperties: "User,Customer").ToList();
            List<SellOrderDto> orderDtos = _mapper.Map<List<SellOrderDto>>(orders);

            return orderDtos;
        }
        public Order Get(Expression<Func<Order, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            return _orderRepo.Get(filter, includeProperties, tracked);
		}

        //public List<OrderDetail> GetDetailsFromOrder(int id)
        //{

        //    var orde _orderDetailRepository.GetDetailsFromOrder(id);
        //}

        public List<SellOrderDto> GetByCustomerName(string text)
        {
            var orders = _orderRepo.GetAll(includeProperties: "Customer").Where(o => o.Customer.Name.Contains(text)).ToList();
            List<SellOrderDto> orderDtos = _mapper.Map<List<SellOrderDto>>(orders);
            return orderDtos;
        }

        public bool Add(Order obj)
		{
			try
			{
				_orderRepo.Add(obj);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
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
