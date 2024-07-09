using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dto
{
    public class ProductToAddV2Dto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public int GoldId { get; set; }
        public decimal GoldWeight { get; set; }
        public string? GemName { get; set; }
        public decimal GemWeight { get; set; }
        public decimal GemPrice { get; set; }
        public decimal Labour { get; set; }
        public int Quantity { get; set; }
        public string? ImgUrl { get; set; }
        public decimal TotalWeight { get; set; }
    }
}
