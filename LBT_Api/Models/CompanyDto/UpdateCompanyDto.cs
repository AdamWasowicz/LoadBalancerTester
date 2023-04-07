namespace LBT_Api.Models.CompanyDto
{
    public class UpdateCompanyDto
    {
        public int? Id { get; set; }
        public int? AddressId { get; set; }
        public int? ContactInfoId { get; set; }
        public string? Name { get; set; }
    }
}
