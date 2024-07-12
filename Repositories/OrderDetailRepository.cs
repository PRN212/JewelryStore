
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
    }
}
