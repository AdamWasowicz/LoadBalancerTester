using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.ProductSoldDto
{
    public class UpdateProductSoldPrice
    {
        [Required]
        public int? Id { get; set; }

        
        // Optional
        public int? AmountSold { get; set; }
        
        public double? PriceAtTheTimeOfSale { get; set; }
    }
}
