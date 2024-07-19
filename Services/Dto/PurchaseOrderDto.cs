namespace Services.Dto
{
	public class PurchaseOrderDto
	{
		public int Id { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string CustomerName { get; set; }
		public string CustomerPhone { get; set; }
		public string CustomerAddress { get; set; }
		public string Status { get; set; }
		public string Type { get; set; }
		public decimal TotalPrice { get; set; }
		public string PaymentMethod { get; set; }
		public int UserId { get; set; }
		public int CustomerId { get; set; }
		public string UserName { get; set; }
		public List<ProductDto> OrderDetails { get; set; } = new List<ProductDto>();
	}
}
