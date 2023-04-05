using LBT_Api.Models.SaleDto;

namespace LBT_Api.Interfaces.Services
{
    public interface ISaleService
    {
        GetSaleDto Create(CreateSaleDto dto);
        public int Delete(int id);
        public GetSaleDto Read(int id);
        public List<GetSaleDto> ReadAll();
        public GetSaleDto Update(UpdateSaleDto dto);
    }
}
