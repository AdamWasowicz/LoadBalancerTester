using LBT_Api.Models.ProductSoldDto;
using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.SaleDto
{
    public class CreateSaleDto
    {
        [Required]
        public int? WorkerId { get; set; }

        [Required]
        public CreateProductSold_IntegratedDto[] ProductsSold { get; set; }
    }
}
