using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.AddressDto;

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
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Check dto fields
            bool dtoIsValid = true;
            if (dto.Country == null || dto.Country.Length == 0)
                dtoIsValid = false;
            else if (dto.City == null || dto.City.Length == 0)
                dtoIsValid = false;
            else if (dto.Street == null || dto.Street.Length == 0)
                dtoIsValid = false;
            else if (dto.BuildingNumber == null || dto.BuildingNumber.Length == 0)
                dtoIsValid = false;

            if (dtoIsValid == false)
                throw new BadRequestException("Dto is missing fields");

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

        public int Delete(int id)
        {
            // Check if record exists
            Address? address = _dbContext.Addresses.FirstOrDefault(a => a.Id == id);
            if (address == null)
                throw new NotFoundException("Delete Address");

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

            return 0;
        }

        public GetAddressDto Read(int id)
        {
            // Check if record exists
            Address? address = _dbContext.Addresses.FirstOrDefault(a => a.Id == id);
            if (address == null)
                throw new NotFoundException("Read Address");

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
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.Id == null)
                throw new BadRequestException("Dto is missing Id field");

            // Check if record exist
            Address? address = _dbContext.Addresses.FirstOrDefault(a => a.Id == dto.Id);
            if (address == null)
                throw new NotFoundException("Update Address");

            // Update record
            if (dto.Country != null)
                address.Country = dto.Country;
            if (dto.City != null)
                address.City = dto.City;
            if (dto.Street != null)
                address.Street = dto.Street;
            if (dto.BuildingNumber != null)
                address.BuildingNumber = dto.BuildingNumber;

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
