
using Repositories.Entities;
using Repositories.IRepositories;

namespace Repositories
{
	public class SellProductRepository : Repository<Product>, ISellProductRepository
	{
		private DataContext _context;
		public SellProductRepository(DataContext db) : base(db)
		{
			_context = db;
		}

		public void Save()
		{
			_context.SaveChanges();
		}

		public void Update(Product obj)
		{
			_context.Products.Update(obj);
		}

	}
}
