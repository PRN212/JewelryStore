
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
	}
}
