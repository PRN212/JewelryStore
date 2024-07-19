using Repositories.Entities;
using Repositories.Specifications.Customers;
using Repositories.IRepositories;

namespace Services
{
    public class CustomerService
    {
        private readonly IGenericRepository<Customer> _customerRepo;
        public CustomerService(IGenericRepository<Customer> customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepo.ListAllAsync();
        }

        public async Task<bool> AddCustomer(Customer customer)
        {
            _customerRepo.Add(customer);
            return await _customerRepo.SaveAllAsync();
        }

        public async Task<Customer?> searchCustomer(string search)
        {
            var param = new CustomerParam()
            {
                Search = search
            };
            var spec = new CustomerSpecification(param);
            return await _customerRepo.GetEntityWithSpec(spec);
        }

        public async Task<Customer> searchCustomerById(int customerId)
        {
            return await _customerRepo.GetByIdAsync(customerId);
        }
    }
}
