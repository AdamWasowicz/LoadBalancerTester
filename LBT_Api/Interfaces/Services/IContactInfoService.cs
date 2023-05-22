using LBT_Api.Models.ContactInfoDto;

namespace LBT_Api.Interfaces.Services
{
    public interface IContactInfoService
    {
        GetContactInfoDto Create(CreateContactInfoDto dto);
        public void Delete(int id);
        public GetContactInfoDto Read(int id);
        public GetContactInfoDto[] ReadAll();
        public GetContactInfoDto Update(UpdateContactInfoDto dto);

        // For testing
        public void CreateExampleData(int amount);
        public int[] GetAllIds();
        public void DeleteRandom();
        public void UpdateRandom();
    }
}
