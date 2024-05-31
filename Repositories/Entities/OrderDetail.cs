

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Entities
{
    public class OrderDetail
    {
        [Key]
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Key]
        [Required]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)"), Required]
        public float Price { get; set; }

    }
}
