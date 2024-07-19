
using Microsoft.EntityFrameworkCore;
using Repositories.Entities.Orders;
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
            var details = _context.OrderDetails.AsNoTracking().Where(o => o.OrderId == id).Include(d => d.Product).ThenInclude(p => p.Gold).ToList();
            return details;
        }
    

        public async Task<IEnumerable<OrderDetail>> GetOrderDetails(int orderId)
        {
            return await _context.OrderDetails
            .Where(p => p.OrderId == orderId)
            .ToListAsync();
        }

        public async Task<bool> AddOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<OrderDetail> GetOrderDetailByProductId(int productId)
        {
            return await _context.OrderDetails
                .Where (p => p.ProductId == productId) 
                .FirstAsync();
        }
    }
}
