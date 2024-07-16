using Repositories.Entities.Orders;

namespace Repositories.IRepositories
{

    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        void Update(OrderDetail obj);
        void Save();
    }
}
