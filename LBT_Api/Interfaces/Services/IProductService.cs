using LBT_Api.Models.ProductDto;

namespace LBT_Api.Interfaces.Services
{
    public interface IProductService
    {
        GetProductDto Create(CreateProductDto dto);
        GetProductWithDependenciesDto CreateWithDependencies(CreateProductWithDependenciesDto dto);
        public void Delete(int id);
        public GetProductDto Read(int id);
        public GetProductWithDependenciesDto ReadWithDependencies(int id);
        public GetProductDto[] ReadAll();
        public GetProductWithDependenciesDto[] ReadAllWithDependencies();
        public GetProductDto Update(UpdateProductDto dto);
    }
}
