

namespace Repositories.Specifications.Customers
{
    public class CustomerParam
    {
        private string _search;

        public string Search
        {
            get => _search;
            set => _search = value.ToLower();
        }
    }
}
