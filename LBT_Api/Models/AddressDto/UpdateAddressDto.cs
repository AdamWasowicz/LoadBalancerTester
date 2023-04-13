using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.AddressDto
{
    public class UpdateAddressDto
    {
        [Required]
        public int? Id { get; set; }

        // Optional
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? BuildingNumber { get; set; }
    }
}
