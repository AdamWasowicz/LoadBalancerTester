using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.AddressDto;
using LBT_Api.Models.CompanyDto;
using LBT_Api.Other;
using System.Net;
using System.Reflection;

namespace LBT_Api.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly LBT_DbContext _dbContext;
        private readonly IMapper _mapper;

        public CompanyService(LBT_DbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public GetCompanyDto Create(CreateCompanyDto dto)
        {
            // Check dto
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Check dto fields
            bool dtoIsValid = Tools.AllStringPropsAreNotNull(dto);
            if (dtoIsValid == false)
                throw new BadRequestException("Dto is missing fields");

            // Create record
            Company company = _mapper.Map<Company>(dto);
            try
            {
                _dbContext.Companys.Add(company);
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }

            GetCompanyDto outputDto = _mapper.Map<GetCompanyDto>(company);
            return outputDto;

        }

        public int Delete(int id)
        {
            Company? company = _dbContext.Companys.FirstOrDefault(c => c.Id == id);
            if (company == null)
                throw new NotFoundException("Company with Id: " + id);

            // Delete record
            try
            {
                _dbContext.Companys.Remove(company);
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }

            return 0;
        }

        public GetCompanyDto Read(int id)
        {
            Company? company = _dbContext.Companys.FirstOrDefault(c => c.Id == id);
            if (company == null)
                throw new NotFoundException("Company with Id: " + id);

            GetCompanyDto outputDto = _mapper.Map<GetCompanyDto>(company);
            
            return outputDto;
        }

        public GetCompanyDto[] ReadAll()
        {
            Company[] companys = _dbContext.Companys.ToArray();
            GetCompanyDto[] companysDto = _mapper.Map<GetCompanyDto[]>(companys);

            return companysDto;
        }

        public GetCompanyDto Update(UpdateCompanyDto dto)
        {
            // Check dto
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.Id == null)
                throw new BadRequestException("Dto is missing Id field");

            // Check if record exist
            Company? company = _dbContext.Companys.FirstOrDefault(c => c.Id == dto.Id);
            if (company == null)
                throw new NotFoundException("Company with Id: " + dto.Id);

            Company mappedComapnyFromDto = _mapper.Map<Company>(dto);
            company = Tools.UpdateObjectProperties(company, mappedComapnyFromDto);

            // Save changes
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }

            GetCompanyDto outputDto = _mapper.Map<GetCompanyDto>(company);

            return outputDto;
        }
    }
}
