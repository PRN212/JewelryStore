using Microsoft.EntityFrameworkCore;
using Repositories.Entities.Orders;
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

		public async Task<IReadOnlyList<Order>> GetPurchaseOrders()
		{
			return await _context.Orders
			.Where(p => p.Type == "Purchase Order")
			.Include(p => p.Customer)
			.Include(p => p.User)
			.Include(p => p.OrderDetails)
			.ThenInclude(o => o.Product)
			.ThenInclude(x => x.Gold)
			.ToListAsync();
		}

		public Order? GetPurchaseOrderById(int id)
		{
			return _context.Orders
			.Where(p => p.Type == "Purchase Order")
			.Include(p => p.Customer)
			.Include(p => p.User)
			.FirstOrDefault(p => p.Id == id);
		}

		public async Task<IEnumerable<Order>> GetPurchaseOrderByCustomerName(string CustomerName)
		{
			return await _context.Orders
			.Where(p => p.Type == "Purchase Order")
			.Include(p => p.Customer.Name == CustomerName)
			.Include(p => p.User)
			.ToListAsync();
		}
		public bool AddPurchaseOrder(Order order)
		{
			_context.Add(order);
			return _context.SaveChanges() > 0;
		}

		public async Task<bool> UpdatePurchaseOrder(Order order)
		{
			_context.Update(order);
			return _context.SaveChanges() > 0;
		}

		public async Task<IEnumerable<Order>> SearchPurchaseOrders(string searchValue)
		{
			return await _context.Orders.AsNoTracking()
				.Where(p => p.Customer.Name.Contains(searchValue))
				.ToListAsync();
		}

		public async Task<IEnumerable<Order>> SearchPurchaseOrdersByDate(DateTime searchDate)
		{
			return await _context.Orders.AsNoTracking()
				.Where(p => p.CreatedDate.Date == searchDate.Date) // So sánh chỉ phần ngày, bỏ qua phần giờ.
				.ToListAsync();
		}
	}
}
