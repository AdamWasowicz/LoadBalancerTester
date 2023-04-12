using LBT_Api.Models.AddressDto;
using LBT_Api.Models.ContactInfoDto;
using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.CompanyDto
{
    public class GetCompanyWithDependenciesDto
    {
        // Props
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Dependencies
        [Required]
        public GetAddressDto Address { get; set; }

        [Required]
        public GetContactInfoDto ContactInfo { get; set; }
    }
}
