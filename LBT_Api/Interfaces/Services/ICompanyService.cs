using LBT_Api.Models.CompanyDto;

namespace LBT_Api.Interfaces.Services
{
    public interface ICompanyService
    {
        public GetCompanyDto Create(CreateCompanyDto dto);
        public GetCompanyWithDependenciesDto CreateWithDependencies(CreateCompanyWithDependenciesDto dto);
        public void Delete(int id);
        public GetCompanyDto Read(int id);
        public GetCompanyWithDependenciesDto ReadWithDependencies(int id);
        public GetCompanyDto[] ReadAll();
        public GetCompanyWithDependenciesDto[] ReadAllWithDependencies();
        public GetCompanyDto UpdateName(UpdateCompanyNameDto dto);

        // For testing
        public void CreateExampleData(int amount);
        public int[] GetAllIds();
        public void DeleteRandom();
        public void UpdateRandom();
    }
}
