using LBT_Api.Models.ProductSoldDto;

namespace LBT_Api.Interfaces.Services
{
    public interface IProductSoldService
    {
        GetProductSoldDto Create(CreateProductSold_SoloDto dto);
        public int Delete(int id);
        public GetProductSoldDto Read(int id);
        public GetProductSoldDto[] ReadAll();
        public GetProductSoldDto Update(UpdateProductSoldDto dto);
    }
}
