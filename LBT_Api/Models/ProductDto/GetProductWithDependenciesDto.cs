using LBT_Api.Models.SupplierDto;
using System.ComponentModel.DataAnnotations;

namespace LBT_Api.Models.ProductDto
{
    public class GetProductWithDependenciesDto
    {
        // Props
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double PriceNow { get; set; }

        // Dependencies
        [Required]
        public GetSupplierWithDependenciesDto Supplier { get; set; }
    }
}
