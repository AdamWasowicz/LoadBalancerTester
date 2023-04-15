using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.ProductSoldDto
{
    public class CreateProductSoldDto
    {
        // Props
        [Required]
        public int? AmountSold { get; set; }

        // Dependencies

        [Required]
        public int? SaleId { get; set; }

        [Required]
        public int? ProductId { get; set; }
    }
}
