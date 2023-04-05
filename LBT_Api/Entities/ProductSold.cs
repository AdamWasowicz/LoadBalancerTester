using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBT_Api.Entities
{
    public class ProductSold
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }


        [Required]
        public int AmountSold { get; set; }

        [Required]
        public double PriceAtTheTimeOfSale { get; set; }
    }
}
