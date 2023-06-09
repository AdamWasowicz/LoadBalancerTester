﻿using LBT_Api.Models.ProductDto;
using LBT_Api.Models.SaleDto;
using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.ProductSoldDto
{
    public class GetProductSoldWithDependenciesDto
    {
        // Props
        [Required]
        public int Id { get; set; }

        [Required]
        public int AmountSold { get; set; }

        [Required]
        public double PriceAtTheTimeOfSale { get; set; }

        // Dependencies
        [Required]
        public GetProductWithDependenciesDto Product { get; set; }

        [Required]
        public GetSaleWithDependenciesDto Sale { get; set; }
    }
}
