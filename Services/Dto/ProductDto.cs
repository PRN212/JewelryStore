
namespace Services.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public int GoldId { get; set; }
        public string GoldType { get; set; } = "";
        public decimal GoldWeight { get; set; }
        public decimal GoldPrice { get; set; }
        public decimal TotalWeight { get; set; }
        public string? GemName { get; set; }
        public decimal GemWeight { get; set; }
        public decimal GemPrice { get; set; }
        public decimal Labour { get; set; }
        public int Quantity { get; set; }
        public string? ImgUrl { get; set; }
        public decimal ProductPrice 
        { 
            get
            {
                return Math.Floor(GoldPrice*GoldWeight*100 + Labour + GemPrice);
            }
        }
    }
}
