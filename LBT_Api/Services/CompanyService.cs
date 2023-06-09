﻿using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.CompanyDto;
using LBT_Api.Other;
using System.Net;

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
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException("Model is invalid");

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

        public void CreateExampleData(int amount)
        {
            for (int i = 0; i < amount; i++)
                CreateData();
        }

        private void CreateData()
        {
            CreateCompanyWithDependenciesDto dto = new CreateCompanyWithDependenciesDto
            {
                Name = "Company_Name",
                Address = new Models.AddressDto.CreateAddressDto
                {
                    Country = "Address_Country",
                    City = "Address_City",
                    BuildingNumber = "Address_BuildingNumber",
                    Street = "Address_Street"
                },
                ContactInfo = new Models.ContactInfoDto.CreateContactInfoDto
                {
                    Email = "ContactInfo_Email",
                    PhoneNumber = "ContactInfo_PhoneNumber",
                }
            };

            CreateWithDependencies(dto);
        }

        public GetCompanyWithDependenciesDto CreateWithDependencies(CreateCompanyWithDependenciesDto dto)
        {
            // Check dto
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException("Model is invalid");

            Company company = null;

                try
                {
                    // Dependencies
                    Address address = _mapper.Map<Address>(dto.Address);
                    _dbContext.Addresses.Add(address);
                    _dbContext.SaveChanges();

                    ContactInfo ci = _mapper.Map<ContactInfo>(dto.ContactInfo);
                    _dbContext.ContactInfos.Add(ci);
                    _dbContext.SaveChanges();

                    // Main
                    company = new Company()
                    {
                        AddressId = address.Id,
                        ContactInfoId = ci.Id,
                        Name = dto.Name
                    };
                    _dbContext.Companys.Add(company);

                    // Save changes
                    _dbContext.SaveChanges();
                }
                catch (Exception exception)
                {
                    throw new DatabaseOperationFailedException(exception.Message);
                }
            

            // Return dto
            GetCompanyWithDependenciesDto outputDto = _mapper.Map<GetCompanyWithDependenciesDto>(company);

            return outputDto;
        }

        public void Delete(int id)
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
        }

        public GetCompanyDto Read(int id)
        {
            Company? company = _dbContext.Companys.FirstOrDefault(c => c.Id == id);
            if (company == null)
                throw new NotFoundException("Company with Id: " + id);

            GetCompanyDto outputDto = _mapper.Map<GetCompanyDto>(company);
            
            return outputDto;
        }

        public GetCompanyWithDependenciesDto ReadWithDependencies(int id)
        {
            Company? company = _dbContext.Companys.FirstOrDefault(c => c.Id == id);
            if (company == null)
                throw new NotFoundException("Company with Id: " + id);

            GetCompanyWithDependenciesDto outputDto = _mapper.Map<GetCompanyWithDependenciesDto>(company);

            return outputDto;
        }

        public GetCompanyDto[] ReadAll()
        {
            Company[] companys = _dbContext.Companys.ToArray();
            GetCompanyDto[] companysDto = _mapper.Map<GetCompanyDto[]>(companys);

            return companysDto;
        }

        public GetCompanyWithDependenciesDto[] ReadAllWithDependencies()
        {
            Company[] companys = _dbContext.Companys.ToArray();
            GetCompanyWithDependenciesDto[] companysDto = _mapper.Map<GetCompanyWithDependenciesDto[]>(companys);

            return companysDto;
        }

        public GetCompanyDto UpdateName(UpdateCompanyNameDto dto)
        {
            // Check dto
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException("Model is invalid");

            // Check if record exist
            Company? company = _dbContext.Companys.FirstOrDefault(c => c.Id == dto.Id);
            if (company == null)
                throw new NotFoundException("Company with Id: " + dto.Id);

            company.Name = dto.Name;

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

        public int[] GetAllIds()
        {
            var ids = _dbContext.Companys.AsQueryable().Select(a => a.Id).ToArray();
            return ids;
        }

        private Company GetRandomCompany()
        {
            int[] items = _dbContext.Companys.AsQueryable().Select(x => x.Id).ToArray();
            Random rnd = new Random();
            int randomIndex = rnd.Next(0, items.Length - 1);

            return _dbContext.Companys.FirstOrDefault(x => x.Id == items[randomIndex])!;
        }

        public void DeleteRandom()
        {
            var item = GetRandomCompany();

            try
            {
                _dbContext.Remove(item);
                _dbContext.SaveChanges();
            }
            catch
            {
                return;
            }
        }

        public void UpdateRandom()
        {
            var item = GetRandomCompany();
            item.Name += "Updated";

            try
            {
                _dbContext.SaveChanges();
            }
            catch
            {
                return;
            }
        }
    }
}
