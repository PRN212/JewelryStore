

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Status { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Type { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public float TotalPrice { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string PaymentMethod { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    }
}
