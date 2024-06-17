﻿
namespace Services.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GoldType { get; set; }
        public float GoldWeight { get; set; }
        public float GoldPrice { get; set; }
        public float TotalWeight { get; set; }
        public string GemName { get; set; }
        public float GemWeight { get; set; }
        public float GemPrice { get; set; }
        public float Labour { get; set; }
        public int Quantity { get; set; }
        public string ImgUrl { get; set; }
        public float ProductPrice { get; set; }
    }
}
