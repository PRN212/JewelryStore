
using Core.Enitities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Entities
{
    public class GoldPrice : BaseEntity
    {
        public int GoldId { get; set; }
        public Gold Gold { get; set; }
        public DateTime DateTime { get; set; }
        [Column(TypeName = "decimal(18,0)")]
        public decimal AskPrice { get; set; }
        [Column(TypeName = "decimal(18,0)")]
        public decimal BidPrice { get; set; }
    }
}
