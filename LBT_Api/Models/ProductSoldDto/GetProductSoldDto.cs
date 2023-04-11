using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.ProductSoldDto
{
    public class GetProductSoldDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int SaleId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int AmountSold { get; set; }

        [Required]
        public double PriceAtTheTimeOfSale { get; set; }
    }
}
