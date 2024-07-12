
using Microsoft.EntityFrameworkCore;
using Repositories.Entities;

namespace Repositories
{
    public class CustomerRepository
    {
        private readonly DataContext _context;
        public CustomerRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Customer>> GetAllCustomerAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public bool AddCustomer(Customer customer)
        {
            _context.Add(customer);
            return _context.SaveChanges() > 0;
        }

        public async Task<Customer?> SearchCustomerByPhoneNumber(string phoneNumber)
        {
            return await _context.Customers
                .Where(c => !string.IsNullOrEmpty(phoneNumber) && c.Phone.Contains(phoneNumber))
                .FirstOrDefaultAsync();
        }
    }
}
