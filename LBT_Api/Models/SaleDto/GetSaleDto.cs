using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.SaleDto
{
    public class GetSaleDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int WorkerId { get; set; }

        [Required]
        public DateTime SaleDate { get; set; }

        [Required]
        public double SumValue { get; set; }
    }
}
