

using Microsoft.EntityFrameworkCore;
using Repositories.Entities.Orders;

namespace Repositories.Specifications.Orders
{
	public class OrderDetailSpecification : BaseSpecification<OrderDetail>
	{
		public OrderDetailSpecification(OrderDetailParam param)
			: base(o => o.ProductId == param.ProductId && o.OrderId == param.OrderId)
		{
			AddCustomInclude(q => q.Include(oi => oi.Product).ThenInclude(p => p.Gold));
		}
	}
}
