namespace Core.Specifications.Orders
{
    public class OrderSpecParams
    {
        private const int MaxPageSize = 10;
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 5;
        public int PageSize 
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
        private string _search;

        public string Search
        {
            get => _search;
            set => _search = value.ToLower();
        }
        public string? OrderType { get; set; }


    }
}
