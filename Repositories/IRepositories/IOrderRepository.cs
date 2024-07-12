using Repositories.Entities;

namespace Repositories.IRepositories
{

	public interface IOrderRepository : IRepository<Order>
	{
		void Update(Order obj);
		void Save();
	}
}
