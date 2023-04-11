using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.AddressDto
{
    public class GetAddressDto 
    {
        [Required]
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
