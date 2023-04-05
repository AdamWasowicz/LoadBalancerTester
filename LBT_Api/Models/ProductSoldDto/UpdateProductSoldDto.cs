﻿namespace LBT_Api.Models.ProductSoldDto
{
    public class UpdateProductSoldDto
    {
        public int Id { get; set; }
        public int? SaleId { get; set; }
        public int? ProductId { get; set; }
        public int? AmountSold { get; set; }
        public double? PriceAtTheTimeOfSale { get; set; }
    }
}