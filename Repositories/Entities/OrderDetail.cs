

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Entities
{
    [PrimaryKey(nameof(ProductId), nameof(OrderId))]
    public class OrderDetail
    {
        //[Key]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        //[Key]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public float Price { get; set; }

    }
}
