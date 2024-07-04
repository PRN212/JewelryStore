using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(200)"), Required]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? Description { get; set; }
        public int GoldId { get; set; }
        public Gold Gold { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal GoldWeight { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalWeight { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string? GemName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal GemWeight { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal GemPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Labour { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string? ImgUrl { get; set; }
        public bool Status { get; set; } = true;

    }
}
