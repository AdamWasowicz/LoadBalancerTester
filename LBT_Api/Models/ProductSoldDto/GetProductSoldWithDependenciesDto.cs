using LBT_Api.Models.SaleDto;
using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.ProductSoldDto
{
    public class GetProductSoldWithDependenciesDto
    {
        // ProductSold props
        [Required]
        public int AmountSold { get; set; }

        [Required]
        public double PriceAtTheTimeOfSale { get; set; }

        // Dependencies
        [Required]
        public GetProductSoldWithDependenciesDto Product { get; set; }

        [Required]
        public GetSaleWithDependenciesDto Sale { get; set; }
    }
}
