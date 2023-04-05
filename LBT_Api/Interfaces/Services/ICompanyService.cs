using LBT_Api.Models.CompanyDto;

namespace LBT_Api.Interfaces.Services
{
    public interface ICompanyService
    {
        GetCompanyDto Create(CreateCompanyDto dto);
        public int Delete(int id);
        public GetCompanyDto Read(int id);
        public List<GetCompanyDto> ReadAll();
        public GetCompanyDto Update(UpdateCompanyDto dto);
    }
}
