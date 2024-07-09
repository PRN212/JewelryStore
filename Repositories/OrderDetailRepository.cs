﻿
using Microsoft.EntityFrameworkCore;
using Repositories.Entities;

namespace Repositories
{
    public class OrderDetailRepository
    {
        private DataContext _context;
        public OrderDetailRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetails(int orderId)
        {
            return await _context.OrderDetail
            .Where(p => p.OrderId == orderId)
            .ToListAsync();
        }

        public bool AddOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetail.Add(orderDetail);
            return _context.SaveChanges() > 0;
        }

        public async Task<OrderDetail> GetOrderDetailByProductId(int productId)
        {
            return await _context.OrderDetail
                .Where (p => p.ProductId == productId) 
                .FirstAsync();
        }
    }
}
