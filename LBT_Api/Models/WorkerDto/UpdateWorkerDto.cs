using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.WorkerDto
{
    public class UpdateWorkerDto
    {
        [Required]
        public int? Id { get; set; }

        // Optional
        public int? CompanyId { get; set; }
        public int? AddressId { get; set; }
        public int? ContactInfoId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
    }
}
