using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.WorkerDto
{
    public class GetWorkerDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public int AddressId { get; set; }

        [Required]
        public int ContactInfoId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }
    }
}
