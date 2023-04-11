using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.CompanyDto
{
    public class UpdateCompanyDto
    {
        [Required]
        public int Id { get; set; }

        // Optional
        public int? AddressId { get; set; }
        public int? ContactInfoId { get; set; }
        public string? Name { get; set; }
    }
}
