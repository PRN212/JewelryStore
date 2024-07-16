

namespace Services.Dto
{
    public class PurchaseOrderDetailDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public string? Description { get; set; }
        public int GoldId { get; set; }
        public string GoldType { get; set; } = "";
        public decimal GoldWeight { get; set; }
        public decimal GoldPrice { get; set; }
        public decimal ProductWeight { get; set; }
        public string? GemName { get; set; }
        public decimal GemWeight { get; set; }
        public decimal GemPrice { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }

    }
}
