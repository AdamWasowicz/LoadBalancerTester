using LBT_Api.Models.ProductSoldDto;
using LBT_Api.Models.WorkerDto;
using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.SaleDto
{
    public class CreateSaleWithDependenciesDto
    {
        // Props
        [Required]
        public CreateProductSold_IntegratedDto ProductsSold { get; set; }

        // Dependencies
        [Required]
        public CreateWorkerWithDependenciesDto Worker { get; set; }
    }
}
