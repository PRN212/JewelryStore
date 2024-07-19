

using Repositories.Entities;

namespace Repositories.Specifications.Customers
{
    public class CustomerSpecification : BaseSpecification<Customer>
    {
        public CustomerSpecification(CustomerParam param) : 
            base (x =>
        (string.IsNullOrEmpty(param.Search) || x.Name.ToLower().Contains(param.Search) || x.Phone.Contains(param.Search)))
        {
        }

        public CustomerSpecification(int id)
        : base(x => x.Id == id)
        {
        }
    }
}
