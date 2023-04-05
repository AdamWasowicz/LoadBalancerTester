namespace LBT_Api.Models.ProductSoldDto
{
    public class GetProductSoldDto
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int AmountSold { get; set; }
        public double PriceAtTheTimeOfSale { get; set; }
    }
}
