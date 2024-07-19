

using Repositories.Enitities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Entities.Orders
{
	public class Order : BaseEntity
	{
		public DateTime CreatedDate { get; set; }
		[Column(TypeName = "nvarchar(50)")]
		public string Status { get; set; } = OrderStatus.Pending.GetEnumMemberValue();
		[Column(TypeName = "nvarchar(50)")]
		public string Type { get; set; }
		[Column(TypeName = "decimal(18,2)")]
		public decimal TotalPrice { get; set; }
		[Column(TypeName = "nvarchar(50)")]
		public string PaymentMethod { get; set; }
		public int CustomerId { get; set; }
		public Customer Customer { get; set; }
		public int UserId { get; set; }
		[ForeignKey("UserId")]
		public User User { get; set; }
		public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
	}
}
