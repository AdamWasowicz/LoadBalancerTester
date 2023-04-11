using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.ProductSoldDto
{
    public class CreateProductSold_IntegratedDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int AmountSold { get; set; }
    }
}
