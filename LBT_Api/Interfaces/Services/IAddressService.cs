using LBT_Api.Models.AddressDto;

namespace LBT_Api.Interfaces.Services
{
    public interface IAddressService
    {
        GetAddressDto Create(CreateAdressDto dto);
        public int Delete(int id);
        public GetAddressDto Read(int id);
        public List<GetAddressDto> ReadAll();
        public GetAddressDto Update(UpdateAddressDto dto);
    }
}
