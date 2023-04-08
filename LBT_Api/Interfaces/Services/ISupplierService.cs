using LBT_Api.Models.SupplierDto;

namespace LBT_Api.Interfaces.Services
{
    public interface ISupplierService
    {
        GetSupplierDto Create(CreateSupplierDto dto);
        public int Delete(int id);
        public GetSupplierDto Read(int id);
        public GetSupplierDto[] ReadAll();
        public GetSupplierDto Update(UpdateSupplierDto dto);
    }
}
