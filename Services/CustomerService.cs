using Repositories.Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Dto;

namespace Services
{
    public class CustomerService
    {
        private readonly CustomerRepository _customerRepo;
        public CustomerService(CustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepo.GetAllCustomerAsync();
        }

        public bool AddCustomer(Customer customer)
        {
            return _customerRepo.AddCustomer(customer);
        }

        public Customer searchCustomerByPhoneNumber(string phoneNumber)
        {
            return _customerRepo.SearchCustomerByPhoneNumber(phoneNumber);
        }
    }
}
