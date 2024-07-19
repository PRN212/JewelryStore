namespace Services.Dto
{
	public class ProductToAddDto
	{
		public string Name { get; set; } = "";
		public string? Description { get; set; }
		public int GoldId { get; set; }
		public decimal GoldWeight { get; set; }
		public string? GemName { get; set; }
		public decimal GemWeight { get; set; }
		public decimal GemPrice { get; set; }
		public decimal Labour { get; set; }
		public int Quantity { get; set; }
		public string? ImgUrl { get; set; }
		public decimal TotalWeight { get; set; }
	}
}
