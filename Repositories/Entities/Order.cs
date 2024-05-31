

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Entities
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "nvarchar(50)"), Required]
        public string Status { get; set; }
        [Column(TypeName = "nvarchar(50)"), Required]
        public string Type { get; set; }
        [Column(TypeName = "decimal(18,2)"), Required]
        public float TotalPrice { get; set; }
        [Column(TypeName = "nvarchar(50)"), Required]
        public string PaymentMethod { get; set; }
        [Required]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    }
}
