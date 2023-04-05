using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Entities
{
    public class ContactInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
