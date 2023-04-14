using LBT_Api.Models.AddressDto;
using LBT_Api.Models.CompanyDto;
using LBT_Api.Models.ContactInfoDto;
using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.WorkerDto
{
    public class CreateWorkerWithDependenciesDto
    {
        // Props
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        // Dependencies
        [Required]
        public CreateCompanyWithDependenciesDto Comapny { get; set; }

        [Required]
        public CreateAddressDto Address { get; set; }

        [Required]
        public CreateContactInfoDto ContactInfo { get; set; }
    }
}
