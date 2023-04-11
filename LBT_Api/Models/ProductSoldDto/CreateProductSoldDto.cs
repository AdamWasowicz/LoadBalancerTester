using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.ProductSoldDto
{
    public class CreateProductSoldDto
    {
        [Required]
        public int SaleId { get; set; }

        [Required]
        public int? ProductId { get; set; }

        [Required]
        public int? AmountSold { get; set; }
    }
}
