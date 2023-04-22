using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.ContactInfoDto;
using LBT_Api.Other;
using System.Net;
using System.Reflection;

namespace LBT_Api.Services
{
    public class ContactInfoService : IContactInfoService
    {
        private readonly LBT_DbContext _dbContext;
        private readonly IMapper _mapper;

        public ContactInfoService(LBT_DbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public GetContactInfoDto Create(CreateContactInfoDto dto)
        {
            // Check dto
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException();

            // Create record
            ContactInfo contactInfo = _mapper.Map<ContactInfo>(dto);
            try
            {
                _dbContext.ContactInfos.Add(contactInfo);
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }

            GetContactInfoDto outputDto = _mapper.Map<GetContactInfoDto>(contactInfo);

            return outputDto;
        }

        public void CreateExampleData(int amount)
        {
            for (int i = 0; i < amount; i++)
                CreateData();
        }

        public void CreateData()
        {
            CreateContactInfoDto dto = new CreateContactInfoDto
            {
                Email = "ContactInfo_Email",
                PhoneNumber = "ContactInfo_PhoneNumber",
            };

            Create(dto);
        }

        public void Delete(int id)
        {
            // Check if record exists
            ContactInfo? contactInfo = _dbContext.ContactInfos.FirstOrDefault(a => a.Id == id);
            if (contactInfo == null)
                throw new NotFoundException("ContactInfo with Id: " + id);

            // Delete record
            try
            {
                _dbContext.ContactInfos.Remove(contactInfo);
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }
        }

        public GetContactInfoDto Read(int id)
        {
            // Check if record exists
            ContactInfo? contactInfo = _dbContext.ContactInfos.FirstOrDefault(a => a.Id == id);
            if (contactInfo == null)
                throw new NotFoundException("ContactInfo with Id: " + id);

            // Return GetContactInfoDto
            GetContactInfoDto outputDto = _mapper.Map<GetContactInfoDto>(contactInfo);

            return outputDto;
        }

        public GetContactInfoDto[] ReadAll()
        {
            ContactInfo[] contactInfos = _dbContext.ContactInfos.ToArray();
            GetContactInfoDto[] contactInfoDtos = _mapper.Map<GetContactInfoDto[]>(contactInfos);

            return contactInfoDtos;
        }

        public GetContactInfoDto Update(UpdateContactInfoDto dto)
        {
            // Check dto
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException();

            // Check if record exist
            ContactInfo? contactInfo = _dbContext.ContactInfos.FirstOrDefault(a => a.Id == dto.Id);
            if (contactInfo == null)
                throw new NotFoundException("ContactInfo with Id: " + dto.Id);

            contactInfo = Tools.UpdateObjectProperties(contactInfo, dto);

            // Save changes
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }

            GetContactInfoDto outputDto = _mapper.Map<GetContactInfoDto>(contactInfo);

            return outputDto;
        }
    }
}
