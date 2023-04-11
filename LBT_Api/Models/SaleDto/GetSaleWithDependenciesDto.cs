using LBT_Api.Models.WorkerDto;
using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.SaleDto
{
    public class GetSaleWithDependenciesDto
    {
        // Props
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime SaleDate { get; set; }

        [Required]
        public double SumValue { get; set; }

        // Dependencies
        [Required]
        public GetWorkerWithDependenciesDto Worker { get; set; }
    }
}
