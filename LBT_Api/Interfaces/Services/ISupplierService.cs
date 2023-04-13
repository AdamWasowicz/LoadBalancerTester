using LBT_Api.Models.SupplierDto;

namespace LBT_Api.Interfaces.Services
{
    public interface ISupplierService
    {
        public GetSupplierDto Create(CreateSupplierDto dto);
        public GetSupplierWithDependenciesDto CreateWithDependencies(CreateSupplierWithDependenciesDto dto);
        public int Delete(int id);
        public GetSupplierDto Read(int id);
        public GetSupplierWithDependenciesDto ReadWithDependencies(int id);
        public GetSupplierDto[] ReadAll();
        public GetSupplierWithDependenciesDto[] ReadAllWithDependencies();
        public GetSupplierDto Update(UpdateSupplierDto dto);
    }
}
