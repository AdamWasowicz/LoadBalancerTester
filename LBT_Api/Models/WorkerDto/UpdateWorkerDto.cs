using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.WorkerDto
{
    public class UpdateWorkerDto
    {
        [Required]
        public int? Id { get; set; }

        // Optional
        public string? Name { get; set; }
        public string? Surname { get; set; }
    }
}
