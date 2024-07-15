using Microsoft.EntityFrameworkCore;
using Repositories.Entities.Orders;

namespace Core.Specifications.Orders
{
    public class OrdersSpecification : BaseSpecification<Order>
    {
        public OrdersSpecification(OrderSpecParams orderSpecParams)
            : base(o => 
                ((string.IsNullOrEmpty(orderSpecParams.Search) 
                || o.Customer.Phone.Contains(orderSpecParams.Search)
                || o.Customer.Name.ToLower().Contains(orderSpecParams.Search))
                && ((string.IsNullOrEmpty(orderSpecParams.OrderType)) 
                || o.Type.ToString() == orderSpecParams.OrderType))
            )
        {
            AddInclude(o => o.User);
            AddInclude(o => o.Customer);
            //AddInclude(o => o.OrderType);
            AddCustomInclude(q => q.Include(o => o.OrderDetails).ThenInclude(oi => oi.Product).ThenInclude(p => p.Gold));
            AddOrderByDescending(o => o.CreatedDate);
        }

        public OrdersSpecification(int id)
            : base(o => o.Id == id)
        {
            AddInclude(o => o.User);
            AddInclude(o => o.Customer);
            //AddInclude(o => o.OrderType);
            AddCustomInclude(q => q.Include(o => o.OrderDetails).ThenInclude(oi => oi.Product).ThenInclude(p => p.Gold));
            AddOrderByDescending(o => o.CreatedDate);
        }

        //public OrdersSpecification(DateOnly today)
        //    : base(o => DateOnly.FromDateTime(o.OrderDate).CompareTo(today) == 0
        //        && o.OrderTypeId == 1
        //        && o.Status.Equals(OrderStatus.PaymentReceived.GetEnumMemberValue()))
        //{
        //    AddInclude(o => o.Promotion);
        //    AddInclude(o => o.Membership);
        //    AddCustomInclude(q => q.Include(o => o.OrderItems).ThenInclude(oi => oi.OrderItemGems));
        //}
    }
}
