
namespace Services.Dto
{
	public class SellOrderDto
	{
		public int Id { get; set; }
		public DateTime CreatedDate { get; set; }
		public string Status { get; set; }
		public string Type { get; set; }
		public float TotalPrice { get; set; }
		public string PaymentMethod { get; set; }
		public string UserName { get; set; }
		public string Customer { get; set; }
	}
}
