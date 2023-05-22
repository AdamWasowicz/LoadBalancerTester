using LBT_Api.Models.AddressDto;

namespace LBT_Api.Interfaces.Services
{
    public interface IAddressService
    {
        GetAddressDto Create(CreateAddressDto dto);
        public void Delete(int id);
        public GetAddressDto Read(int id);
        public GetAddressDto[] ReadAll();
        public GetAddressDto Update(UpdateAddressDto dto);

        // For testing
        public void CreateExampleData(int amount);
        public int[] GetAllIds();
        public void DeleteRandom();
        public void UpdateRandom(); 
    }
}
