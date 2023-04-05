using LBT_Api.Models.ProductSoldDto;

namespace LBT_Api.Interfaces.Services
{
    public interface IProductSoldDto
    {
        GetProductSoldDto Create(CreateProductSoldDto dto);
        public int Delete(int id);
        public GetProductSoldDto Read(int id);
        public List<GetProductSoldDto> ReadAll();
        public GetProductSoldDto Update(UpdateProductSoldDto dto);
    }
}
