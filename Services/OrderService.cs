
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Entities;
using Static;
using System.Linq.Expressions;

namespace Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepo;
        public OrderService(OrderRepository orderRepo) 
        {
            _orderRepo = orderRepo;
        }
        public List<Order> GetAll(Expression<Func<Order, bool>>? filter = null, string? includeProperties = null) { 
            return _orderRepo.GetAll().ToList();
		}

        public List<Order> GetAllSellOrders()
        {
            return _orderRepo.GetAll(o => o.Type == SD.TypeSell, includeProperties: "OrderDetails").ToList();
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
