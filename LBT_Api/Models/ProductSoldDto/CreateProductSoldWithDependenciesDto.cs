using LBT_Api.Models.ProductDto;
using LBT_Api.Models.SaleDto;
using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.ProductSoldDto
{
    public class CreateProductSoldWithDependenciesDto
    {
        // Props
        [Required]
        public int AmountSold { get; set; }

        // Dependencies
        [Required]
        public CreateProductWithDependenciesDto Product { get; set; }

        [Required]
        public CreateSaleWithDependenciesDto Sale { get; set; }
    }
}
