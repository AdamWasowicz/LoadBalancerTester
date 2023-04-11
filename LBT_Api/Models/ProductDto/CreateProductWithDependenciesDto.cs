using LBT_Api.Models.SupplierDto;
using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.ProductDto
{
    public class CreateProductWithDependenciesDto
    {
        // Props
        [Required]
        public string Name { get; set; }

        [Required]
        public double PriceNow { get; set; }

        // Dependencies
        public CreateSupplierWithDependenciesDto Supplier { get; set; }
    }
}
