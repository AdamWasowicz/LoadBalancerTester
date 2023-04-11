using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.SaleDto
{
    public class UpdateSaleDto
    {
        [Required]
        public int Id { get; set; }

        // Optional
        public int? WorkerId { get; set; }
    }
}
