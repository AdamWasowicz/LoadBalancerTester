namespace LBT_Api.Models.ProductDto
{
    public class UpdateProductDto
    {
        public int? Id { get; set; }
        public int? SupplierId { get; set; }
        public string? Name { get; set; }
        public double? PriceNow { get; set; }
    }
}
