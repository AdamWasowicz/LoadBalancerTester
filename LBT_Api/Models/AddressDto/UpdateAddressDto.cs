namespace LBT_Api.Models.AddressDto
{
    public class UpdateAddressDto
    {
        public int? Id { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? BuildingNumber { get; set; }
    }
}
