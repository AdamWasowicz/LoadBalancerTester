using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.AddressDto;
using LBT_Api.Models.CompanyDto;
using LBT_Api.Models.ContactInfoDto;
using LBT_Api.Models.SaleDto;
using LBT_Api.Models.WorkerDto;
using LBT_Api.Other;

namespace LBT_Api.Services
{
    public class SaleService : ISaleService
    {
        private readonly LBT_DbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IWorkerService _workerService;

        public SaleService(
            LBT_DbContext dbContext, 
            IMapper mapper, 
            IWorkerService workerService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _workerService = workerService;
        }

        public GetSaleDto Create(CreateSaleDto dto)
        {
            // Check dto
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException();

            bool workerExists = _dbContext.Workers.Where(w => w.Id == dto.WorkerId).Any();
            if (workerExists == false)
                throw new NotFoundException("Worker with Id: " + dto.WorkerId);

            // Create records
            Sale sale = new Sale()
            {
                SaleDate = DateTime.Now,
                WorkerId = (int)dto.WorkerId,
                SumValue = 0,
            };

            _dbContext.Sales.Add(sale);
            _dbContext.SaveChanges();

            // Return dto
            GetSaleDto outputDto = _mapper.Map<GetSaleDto>(sale);

            return outputDto;
        }

        public void CreateExampleData(int amount)
        {
            for (int i = 0; i < amount; i++)
                CreateData();
        }

        private void CreateData()
        {
            CreateSaleWithDependenciesDto dto = new CreateSaleWithDependenciesDto
            {
                Worker = new CreateWorkerWithDependenciesDto
                {
                    Name = "Worker_Name",
                    Surname = "Worker_Surname",
                    Address = new CreateAddressDto
                    {
                        City = "Address_City",
                        Country = "Address_Country",
                        BuildingNumber = "Address_BuildingNumber",
                        Street = "Address_Street"
                    },// Address
                    ContactInfo = new CreateContactInfoDto
                    {
                        Email = "ContactInfo_Email",
                        PhoneNumber = "ContactInfo_PhoneNumber"
                    },// ContactInfo
                    Comapny = new CreateCompanyWithDependenciesDto
                    {
                        Name = "Company_Name",
                        Address = new CreateAddressDto
                        {
                            City = "Address_City",
                            Country = "Address_Country",
                            BuildingNumber = "Address_BuildingNumber",
                            Street = "Address_Street"
                        },// Address
                        ContactInfo = new CreateContactInfoDto
                        {
                            Email = "ContactInfo_Email",
                            PhoneNumber = "ContactInfo_PhoneNumber"
                        },// ContactInfo
                    }// Company
                }// Worker
            };// Sale

            CreateWithDependencies(dto);
        }

        public GetSaleWithDependenciesDto CreateWithDependencies(CreateSaleWithDependenciesDto dto)
        {
            // Check dto
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException("Model is invalid");

            Sale sale = null;
                try
                {
                    // Dependencies
                    int workerId = _workerService.CreateWithDependencies(dto.Worker).Id;

                    // Main
                    sale = new Sale
                    {
                        SaleDate = DateTime.Now,
                        WorkerId = workerId,
                        SumValue = 0,
                    };

                    // Save changes
                    _dbContext.Sales.Add(sale);
                    _dbContext.SaveChanges();
                }
                catch (Exception exception)
                {
                    throw new DatabaseOperationFailedException(exception.Message);
                }
            

            // Return dto
            GetSaleWithDependenciesDto outputDto = _mapper.Map<GetSaleWithDependenciesDto>(sale);

            return outputDto;
        }

        public void Delete(int id)
        {
            // Check if record exists
            Sale? sale = _dbContext.Sales.FirstOrDefault(s => s.Id == id);
            if (sale == null)
                throw new NotFoundException("Sale with Id: " + id);

            // Delete records
            var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.Sales.Remove(sale);
                _dbContext.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public GetSaleDto Read(int id)
        {
            // Check if record exists
            Sale? sale = _dbContext.Sales.FirstOrDefault(a => a.Id == id);
            if (sale == null)
                throw new NotFoundException("Sale with Id: " + id);

            // Return dto
            GetSaleDto outputDto = _mapper.Map<GetSaleDto>(sale);

            return outputDto;
        }

        public GetSaleWithDependenciesDto ReadWithDependencies(int id)
        {
            // Check if record exists
            Sale? sale = _dbContext.Sales.FirstOrDefault(a => a.Id == id);
            if (sale == null)
                throw new NotFoundException("Sale with Id: " + id);

            // Return dto
            GetSaleWithDependenciesDto outputDto = _mapper.Map<GetSaleWithDependenciesDto>(sale);

            return outputDto;
        }

        public GetSaleDto[] ReadAll()
        {
            Sale[] sales = _dbContext.Sales.ToArray();
            GetSaleDto[] saleDtos = _mapper.Map<GetSaleDto[]>(sales);

            return saleDtos;
        }

        public GetSaleWithDependenciesDto[] ReadAllWithDependencies()
        {
            Sale[] sales = _dbContext.Sales.ToArray();
            GetSaleWithDependenciesDto[] saleDtos = _mapper.Map<GetSaleWithDependenciesDto[]>(sales);

            return saleDtos;
        }

        public GetSaleDto Update(UpdateSaleDto dto)
        {
            // Check dto
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException();

            // Check if records exist
            Sale? sale = _dbContext.Sales.FirstOrDefault(s => s.Id == dto.Id);
            if (sale == null)
                throw new NotFoundException("Sale with Id: " + dto.Id);

            Worker? worker = _dbContext.Workers.FirstOrDefault(w => w.Id == dto.WorkerId) ?? throw new NotFoundException("Worker with Id: " + dto.Id);
            sale = Tools.UpdateObjectProperties(sale, dto);

            // Save changes
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }

            GetSaleDto outputDto = _mapper.Map<GetSaleDto>(sale);

            return outputDto;
        }

        public int[] GetAllIds()
        {
            var ids = _dbContext.Sales.AsQueryable().Select(a => a.Id).ToArray();
            return ids;
        }
    }
}
