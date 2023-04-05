using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBT_Api.Entities
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey("Worker")]
        public int WorkerId { get; set; }
        public virtual Worker Worker { get; set; }


        [Required]
        public DateTime SaleDate { get; set; }

        [Required]
        public double SumValue { get; set; }
    }
}
