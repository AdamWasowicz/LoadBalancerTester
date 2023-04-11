using LBT_Api.Models.CompanyDto;

namespace LBT_Api.Interfaces.Services
{
    public interface ICompanyService
    {
        public GetCompanyDto Create(CreateCompanyDto dto);
        //public GetCompanyDto CreateWithDependencies(CreateCompanyWithDependencies dto);
        public int Delete(int id);
        public GetCompanyDto Read(int id);
        //public GetCompanyWithDependencies ReadWithDependencies(int id);
        public GetCompanyDto[] ReadAll();
        //public GetCompanyWithDependencies[] ReadAllWithDependencies();
        public GetCompanyDto Update(UpdateCompanyDto dto);
    }
}
