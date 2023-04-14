using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.SupplierDto
{
    public class UpdateSupplierDto
    {
        [Required]
        public int? Id { get; set; }
        
        // Optional
        public string? Name { get; set; }
    }
}
