using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.ProductDto
{
    public class GetProductDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double PriceNow { get; set; }
    }
}
