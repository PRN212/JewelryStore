using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class OrderDetail_ProductOrderDetailRepository
    {
        private DataContext _context;
        public OrderDetail_ProductOrderDetailRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetails(int orderId)
        {
            return await _context.OrderDetail
            .Where(p => p.OrderId == orderId)
            .ToListAsync();
        }

        public bool AddOrderDetail_ProductOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetail.Add(orderDetail);
            return _context.SaveChanges() > 0;
        }
    }
}
