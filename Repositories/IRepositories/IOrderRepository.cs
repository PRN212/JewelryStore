using Repositories.Entities.Orders;

namespace Repositories.IRepositories
{

    public interface IOrderRepository : IRepository<Order>
	{
		void Update(Order obj);
		void Save();
	}
}
