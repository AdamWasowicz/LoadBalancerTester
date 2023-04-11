using LBT_Api.Models.AddressDto;
using LBT_Api.Models.CompanyDto;
using LBT_Api.Models.ContactInfoDto;
using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.WorkerDto
{
    public class GetWorkerWithDependenciesDto
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
        public GetCompanyWithDependencies Company { get; set; }

        [Required]
        public GetAddressDto Address { get; set; }

        [Required]
        public GetContactInfoDto ContactInfo { get; set; }
    }
}
