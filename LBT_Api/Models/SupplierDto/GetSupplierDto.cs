using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.SupplierDto
{
    public class GetSupplierDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int AddressId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
