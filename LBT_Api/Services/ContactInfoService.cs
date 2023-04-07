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
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Check dto fields
            bool dtoIsValid = Tools.AllStringPropsAreNotNull(dto);
            if (dtoIsValid == false)
                throw new BadRequestException("Dto is missing fields");

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

        public int Delete(int id)
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

            return 0;
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
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.Id == null)
                throw new BadRequestException("Dto is missing Id field");

            // Check if record exist
            ContactInfo? contactInfo = _dbContext.ContactInfos.FirstOrDefault(a => a.Id == dto.Id);
            if (contactInfo == null)
                throw new NotFoundException("ContactInfo with Id: " + dto.Id);

            ContactInfo mappedContactInfoFromDto = _mapper.Map<ContactInfo>(dto);
            contactInfo = Tools.UpdateObjectProperties(contactInfo, mappedContactInfoFromDto);

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
