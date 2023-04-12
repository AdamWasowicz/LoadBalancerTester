using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.CompanyDto
{
    public class UpdateCompanyNameDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
