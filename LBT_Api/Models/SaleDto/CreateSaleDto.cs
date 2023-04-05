namespace LBT_Api.Models.SaleDto
{
    public class CreateSaleDto
    {
        public int WorkerId { get; set; }
        public DateTime SaleDate { get; set; }
        public double SumValue { get; set; }
    }
}
