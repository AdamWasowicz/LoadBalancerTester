using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Entities
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string BuildingNumber { get; set; }
    }
}
