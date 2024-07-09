
using Repositories.Entities;
using Repositories.IRepositories;
namespace Repositories
{
	public class OrderRepository : Repository<Order>, IOrderRepository
	{
		private DataContext _context;
		public OrderRepository(DataContext db) : base(db)
		{
			_context = db;
		}

		public void Save()
		{
			_context.SaveChanges();
		}

		public void Update(Order obj)
		{
			_context.Orders.Update(obj);
		}
	}
}
