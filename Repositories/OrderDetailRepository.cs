﻿
using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.IRepositories;
namespace Repositories
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
	{
		private DataContext _context;
		public OrderDetailRepository(DataContext db) : base(db)
		{
			_context = db;
		}

		public void Save()
		{
			_context.SaveChanges();
		}

		public void Update(OrderDetail obj)
		{
			_context.OrderDetails.Update(obj);
		}

        public List<OrderDetail> GetDetailsFromOrder(int id)
        {
            var details = _context.OrderDetails.Where(o => o.OrderId == id).Include(d => d.Product).ThenInclude(p => p.Gold).ToList();
            return details;
        }
    

        public async Task<IEnumerable<OrderDetail>> GetOrderDetails(int orderId)
        {
            return await _context.OrderDetail
            .Where(p => p.OrderId == orderId)
            .ToListAsync();
        }

        public bool AddOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetail.Add(orderDetail);
            return _context.SaveChanges() > 0;
        }

        public async Task<OrderDetail> GetOrderDetailByProductId(int productId)
        {
            return await _context.OrderDetail
                .Where (p => p.ProductId == productId) 
                .FirstAsync();
        }
    }
}
