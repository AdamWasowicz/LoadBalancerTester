using LBT_Api.Models.AddressDto;
using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.SupplierDto
{
    public class CreateSupplierWithDependenciesDto
    {
        // Props
        [Required]
        public string Name { get; set; }

        // Dependencies
        [Required]
        public CreateAddressDto Address { get; set; }
    }
}
