namespace LBT_Api.Models.SaleDto
{
    public class GetSaleDto
    {
        public int Id { get; set; }
        public int WorkerId { get; set; }
        public DateTime SaleDate { get; set; }
        public double SumValue { get; set; }
    }
}
