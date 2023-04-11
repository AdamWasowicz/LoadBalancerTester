using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.ProductDto
{
    public class CreateProductDto
    {
        [Required]
        public int SupplierId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double PriceNow { get; set; }
    }
}
