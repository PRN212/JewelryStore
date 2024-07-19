namespace Repositories.Specifications.Products
{
    public class ProductParams
    {
        private string _search;

        public string Search
        {
            get => _search;
            set => _search = value.ToLower();
        }
        public int? GoldTypeId { get; set; }
        public bool Status { get; set; }
    }
}
