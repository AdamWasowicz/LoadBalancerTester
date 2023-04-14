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
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException();

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

        public GetSupplierWithDependenciesDto CreateWithDependencies(CreateSupplierWithDependenciesDto dto)
        {
            // Check dto
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException();

            var transaction = _dbContext.Database.BeginTransaction();
            Supplier supplier = null;

            try
            {
                // Dependencies
                Address address = _mapper.Map<Address>(dto.Address);
                _dbContext.Addresses.Add(address);

                // Main
                supplier = new Supplier
                {
                    AddressId = address.Id,
                    Name = dto.Name,
                };

                _dbContext.Suppliers.Add(supplier);
                _dbContext.SaveChanges();
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw new DatabaseOperationFailedException(exception.Message);
            }

            // Return dto
            GetSupplierWithDependenciesDto outputDto = _mapper.Map<GetSupplierWithDependenciesDto>(supplier);

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

        public GetSupplierWithDependenciesDto ReadWithDependencies(int id)
        {
            // Check if record exists
            Supplier? supplier = _dbContext.Suppliers.FirstOrDefault(s => s.Id == id);
            if (supplier == null)
                throw new NotFoundException("Supplier with Id: " + id);

            // Return Dto
            GetSupplierWithDependenciesDto outputDto = _mapper.Map<GetSupplierWithDependenciesDto>(supplier);

            return outputDto;
        }

        public GetSupplierDto[] ReadAll()
        {
            Supplier[] supplier = _dbContext.Suppliers.ToArray();
            GetSupplierDto[] outputDto = _mapper.Map<GetSupplierDto[]>(supplier);

            return outputDto;
        }

        public GetSupplierWithDependenciesDto[] ReadAllWithDependencies()
        {
            Supplier[] suppliers = _dbContext.Suppliers.ToArray();
            GetSupplierWithDependenciesDto[] outputDto = _mapper.Map<GetSupplierWithDependenciesDto[]>(suppliers);
        
            return outputDto;
         }

        public GetSupplierDto Update(UpdateSupplierDto dto)
        {
            // Check dto
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException();

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
