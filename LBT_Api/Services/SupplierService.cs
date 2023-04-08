using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.SupplierDto;
using LBT_Api.Other;

namespace LBT_Api.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly LBT_DbContext _dbContext;
        private readonly IMapper _mapper;

        public SupplierService(LBT_DbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public GetSupplierDto Create(CreateSupplierDto dto)
        {
            // Check dto
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Check dto fields
            bool dtoIsValid = Tools.AllStringPropsAreNotNull(dto);
            if (dtoIsValid == false)
                throw new BadRequestException("Dto is missing fields");

            // Create record
            Supplier supplier = _mapper.Map<Supplier>(dto);
            try
            {
                _dbContext.Suppliers.Add(supplier);
                _dbContext.SaveChanges();
            }
            catch (Exception exception) 
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }

            GetSupplierDto outputDto = _mapper.Map<GetSupplierDto>(supplier);

            return outputDto;
        }

        public int Delete(int id)
        {
            // Check if record exists
            Supplier? supplier = _dbContext.Suppliers.FirstOrDefault(s => s.Id == id);
            if (supplier == null)
                throw new NotFoundException("Supplier with Id: " + id);

            // Delete record
            try
            {
                _dbContext.Suppliers.Remove(supplier);
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }

            return 0;
        }

        public GetSupplierDto Read(int id)
        {
            // Check if record exists
            Supplier? supplier = _dbContext.Suppliers.FirstOrDefault(s => s.Id == id);
            if (supplier == null)
                throw new NotFoundException("Supplier with Id: " + id);

            // Return Dto
            GetSupplierDto outputDto = _mapper.Map<GetSupplierDto>(supplier);

            return outputDto;
        }

        public GetSupplierDto[] ReadAll()
        {
            Supplier[] supplier = _dbContext.Suppliers.ToArray();
            GetSupplierDto[] outputDtos = _mapper.Map<GetSupplierDto[]>(supplier);

            return outputDtos;
        }

        public GetSupplierDto Update(UpdateSupplierDto dto)
        {
            // Check dto
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.Id == null)
                throw new BadRequestException("Dto is missing Id field");

            // Check if record exist
            Supplier? supplier = _dbContext.Suppliers.FirstOrDefault(s => s.Id == dto.Id);
            if (supplier == null)
                throw new NotFoundException("Supplier with Id: " + dto.Id);

            Supplier mappedFromDto = _mapper.Map<Supplier>(dto);
            supplier = Tools.UpdateObjectProperties(supplier, mappedFromDto);

            // Save changes
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }

            GetSupplierDto outputDto = _mapper.Map<GetSupplierDto>(supplier);

            return outputDto;
        }
    }
}
