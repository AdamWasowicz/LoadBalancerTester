using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.ContactInfoDto
{
    public class GetContactInfoDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
