using LBT_Api.Models.AddressDto;
using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.SupplierDto
{
    public class GetSupplierWithDependenciesDto
    {
        // Props
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Dependencies
        [Required]
        public GetAddressDto Address { get; set; }
    }
}
