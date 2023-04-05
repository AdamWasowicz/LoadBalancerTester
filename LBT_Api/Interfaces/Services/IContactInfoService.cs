using LBT_Api.Models.ContactInfoDto;

namespace LBT_Api.Interfaces.Services
{
    public interface IContactInfoService
    {
        GetContactInfoDto Create(CreateContactInfoDto dto);
        public int Delete(int id);
        public GetContactInfoDto Read(int id);
        public List<GetContactInfoDto> ReadAll();
        public GetContactInfoDto Update(UpdateContactInfoDto dto);
    }
}
