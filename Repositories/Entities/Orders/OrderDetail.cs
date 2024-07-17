

using Repositories.Enitities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Entities.Orders
{
    public class OrderDetail : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,0)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(18,0)")]
        public decimal GoldPrice { get; set; }

    }
}
