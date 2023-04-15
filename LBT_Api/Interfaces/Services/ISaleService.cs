using LBT_Api.Models.SaleDto;

namespace LBT_Api.Interfaces.Services
{
    public interface ISaleService
    {
        public GetSaleDto Create(CreateSaleDto dto);
        public GetSaleWithDependenciesDto CreateWithDependencies(CreateSaleWithDependenciesDto dto);
        public int Delete(int id);
        public GetSaleDto Read(int id);
        public GetSaleWithDependenciesDto ReadWithDependencies(int id);
        public GetSaleDto[] ReadAll();
        public GetSaleWithDependenciesDto[] ReadAllWithDependencies();
        public GetSaleDto Update(UpdateSaleDto dto);
    }
}
