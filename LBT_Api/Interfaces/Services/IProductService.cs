using LBT_Api.Models.ProductDto;

namespace LBT_Api.Interfaces.Services
{
    public interface IProductService
    {
        GetProductDto Create(CreateProductDto dto);
        public int Delete(int id);
        public GetProductDto Read(int id);
        public GetProductDto[] ReadAll();
        public GetProductDto Update(UpdateProductDto dto);
    }
}
