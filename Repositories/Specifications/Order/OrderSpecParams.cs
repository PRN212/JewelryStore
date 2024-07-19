namespace Repositories.Specifications.Orders
{
	public class OrderSpecParams
	{
		private string _search;

		public string Search
		{
			get => _search;
			set => _search = value.ToLower();
		}
		public string? OrderType { get; set; }
		public string? OrderStatus { get; set; }
	}
}
