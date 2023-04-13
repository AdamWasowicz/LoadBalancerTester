using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.ContactInfoDto
{
    public class UpdateContactInfoDto
    {
        [Required]
        public int? Id { get; set; }

        // Optional
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
