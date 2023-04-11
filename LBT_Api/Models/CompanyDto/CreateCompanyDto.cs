using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.CompanyDto
{
    public class CreateCompanyDto
    {
        [Required]
        public int AddressId { get; set; }

        [Required]
        public int ContactInfoId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
