using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.ProductSoldDto
{
    public class UpdateProductSoldDto
    {
        [Required]
        public int? Id { get; set; }

        // Optional
        public int? SaleId { get; set; }
        public int? ProductId { get; set; }
        public int? AmountSold { get; set; }
        public double? PriceAtTheTimeOfSale { get; set; }
    }
}
