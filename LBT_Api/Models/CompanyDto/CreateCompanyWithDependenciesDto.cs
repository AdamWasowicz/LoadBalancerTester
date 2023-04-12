using LBT_Api.Models.AddressDto;
using LBT_Api.Models.ContactInfoDto;
using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.CompanyDto
{
    public class CreateCompanyWithDependenciesDto
    {
        // Company props
        [Required]
        public string Name { get; set; }

        // Dependencies
        [Required]
        public CreateAddressDto Address { get; set; }

        [Required]
        public CreateContactInfoDto ContactInfo { get; set; }
    }
}
