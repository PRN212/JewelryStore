
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Entities
{
    public class GoldPrice
    {
        public int Id { get; set; }
        [Required]
        public int GoldId { get; set; }
        public Gold Gold {  get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Column(TypeName = "decimal(18,2)"), Required]
        public float AskPrice { get; set; }
        [Column(TypeName = "decimal(18,2)"), Required]
        public float BidPrice { get; set; }
        [Column(TypeName = "decimal(5,2)"), Required]
        public float AskRate { get; set; }
        [Column(TypeName = "decimal(5,2)"), Required]
        public float BidRate { get; set;}
    }
}
