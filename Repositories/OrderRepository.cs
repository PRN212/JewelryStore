
using Microsoft.EntityFrameworkCore;
using Repositories.Entities;

namespace Repositories
{
    public class OrderRepository
    {
        private DataContext _context;
        public OrderRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetPurchaseOrders()
        {
            return await _context.Orders
            .Where(p => p.Type == "Purchase Order" && p.Status != "Disabled")
            .Include(p => p.Customer)
            .Include(p => p.User)
            .ToListAsync();
        }

        public Order? GetPurchaseOrderById(int id)
        {
            return _context.Orders
            .Where(p => p.Type == "Purchase Order")
            .Include(p => p.Customer)
            .Include(p => p.User)
            .FirstOrDefault(p => p.Id == id);
        }

        public async Task<IEnumerable<Order>> GetPurchaseOrderByCustomerName(string CustomerName)
        {
            return await _context.Orders
            .Where(p => p.Type == "Purchase Order")
            .Include(p => p.Customer.Name == CustomerName)
            .Include(p => p.User)
            .ToListAsync();
        }
        public bool AddPurchaseOrder(Order order)
        {
            _context.Add(order);
            return _context.SaveChanges() > 0;
        }

        public bool DeletePurchaseOrder(Order order)
        {
            // Retrieve the order from the context
            var ordered = _context.Orders.SingleOrDefault(o => o.Id == order.Id);

            if (ordered == null)
            {
                // Order not found
                return false;
            }

            // Set the Status property to "Disabled"
            ordered.Status = "Disabled";

            // Save the changes to the database
            return _context.SaveChanges() > 0;
        }
        
        public bool IsPaid(Order order)
        {
            // Retrieve the order from the context
            var ordered = _context.Orders.SingleOrDefault(o => o.Id == order.Id);

            if (ordered == null)
            {
                // Order not found
                return false;
            }

            // Set the Status property to "Disabled"
            ordered.Status = "Paid";

            // Save the changes to the database
            return _context.SaveChanges() > 0;
        }

        public async Task<IEnumerable<Order>> SearchPurchaseOrders(string searchValue)
        {
            return await _context.Orders
                .Where(p => p.Customer.Name.Contains(searchValue))
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> SearchPurchaseOrdersByDate(DateTime searchDate)
        {
            return await _context.Orders
                .Where(p => p.CreatedDate.Date == searchDate.Date) // So sánh chỉ phần ngày, bỏ qua phần giờ.
                .ToListAsync();
        }
    }
}
