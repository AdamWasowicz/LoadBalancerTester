using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.ContactInfoDto
{
    public class CreateContactInfoDto
    {
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
