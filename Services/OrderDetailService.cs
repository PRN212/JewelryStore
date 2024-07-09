
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Entities;
using System.Linq.Expressions;

namespace Services
{
    public class OrderDetailService
    {
        private readonly OrderDetailRepository _orderRepo;
        public OrderDetailService(OrderDetailRepository orderRepo) 
        {
            _orderRepo = orderRepo;
        }
        public List<OrderDetail> GetAll(Expression<Func<OrderDetail, bool>>? filter = null, string? includeProperties = null) { 
            return _orderRepo.GetAll().ToList();
		}

        public OrderDetail Get(Expression<Func<OrderDetail, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            return _orderRepo.Get(filter, includeProperties, tracked);
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
