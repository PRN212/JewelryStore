using Repositories.Entities;

namespace Repositories.IRepositories
{

	public interface ISellProductRepository : IRepository<Product>
	{
		void Update(Product obj);
		void Save();
	}
}
