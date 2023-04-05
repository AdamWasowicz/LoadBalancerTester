using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBT_Api.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }


        [Required]
        public string Name { get; set; }

        [Required]
        public double PriceNow { get; set; }


        public virtual List<Product> Products { get; set; }
    }
}
