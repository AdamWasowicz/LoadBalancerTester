using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.AddressDto;
using LBT_Api.Other;
using System.Reflection;

namespace LBT_Api.Services
{
    public class AddressService : IAddressService
    {
        private readonly LBT_DbContext _dbContext;
        private readonly IMapper _mapper;

        public AddressService(LBT_DbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public GetAddressDto Create(CreateAddressDto dto)
        {
            // Check dto
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException();

            // Create record
            Address address = _mapper.Map<Address>(dto);
            try
            {
                _dbContext.Addresses.Add(address);
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }

            GetAddressDto outputDto = _mapper.Map<GetAddressDto>(address);
            
            return outputDto;
        }

        public void Delete(int id)
        {
            // Check if record exists
            Address? address = _dbContext.Addresses.FirstOrDefault(a => a.Id == id);
            if (address == null)
                throw new NotFoundException("Address with Id: " + id);

            // Delete record
            try
            {
                _dbContext.Addresses.Remove(address);
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }
        }

        public GetAddressDto Read(int id)
        {
            // Check if record exists
            Address? address = _dbContext.Addresses.FirstOrDefault(a => a.Id == id);
            if (address == null)
                throw new NotFoundException("Address with Id: " + id);

            // Return GetAddressDto
            GetAddressDto outputDto = _mapper.Map<GetAddressDto>(address);
            
            return outputDto;
        }

        public GetAddressDto[] ReadAll()
        {
            Address[] addresses = _dbContext.Addresses.ToArray();
            GetAddressDto[] addressesDto = _mapper.Map<GetAddressDto[]>(addresses);
            
            return addressesDto;
        }

        public GetAddressDto Update(UpdateAddressDto dto)
        {
            // Check dto
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException();

            // Check if record exist
            Address? address = _dbContext.Addresses.FirstOrDefault(a => a.Id == dto.Id);
            if (address == null)
                throw new NotFoundException("Address with Id: " + dto.Id);

            Address mappedAddressFromDto = _mapper.Map<Address>(dto);
            address = Tools.UpdateObjectProperties(address, mappedAddressFromDto);

            // Save changes
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }

            GetAddressDto outputDto = _mapper.Map<GetAddressDto>(address);
            
            return outputDto;
        }
    }
}
