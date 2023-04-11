using LBT_Api.Models.AddressDto;
using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.CompanyDto
{
    public class CreateCompanyWithDependencies
    {
        // Company props
        [Required]
        public string Name { get; set; }

        // Dependencies
        [Required]
        public CreateAddressDto Address { get; set; }
    }
}
