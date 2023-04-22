using LBT_Api.Models.ProductDto;
using LBT_Api.Models.ProductSoldDto;

namespace LBT_Api.Interfaces.Services
{
    public interface IProductSoldService
    {
        public GetProductSoldDto Create(CreateProductSoldDto dto);
        public GetProductSoldWithDependenciesDto CreateWithDependencies(CreateProductSoldWithDependenciesDto dto);
        public void Delete(int id);
        public GetProductSoldDto Read(int id);
        public GetProductSoldWithDependenciesDto ReadWithDependencies(int id);
        public GetProductSoldDto[] ReadAll();
        public GetProductSoldWithDependenciesDto[] ReadAllWithDependencies();
        public GetProductSoldDto Update(UpdateProductSoldPrice dto);

        // Seed
        public void CreateExampleData(int amount);
    }
}
