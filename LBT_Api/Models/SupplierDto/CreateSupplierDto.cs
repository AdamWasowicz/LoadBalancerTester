using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.SupplierDto
{
    public class CreateSupplierDto
    {
        [Required]
        public int AddressId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
