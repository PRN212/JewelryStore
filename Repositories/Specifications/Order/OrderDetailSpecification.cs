

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Repositories.Entities.Orders;
using System.Linq.Expressions;

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
