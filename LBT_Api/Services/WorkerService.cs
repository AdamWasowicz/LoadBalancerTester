using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.AddressDto;
using LBT_Api.Models.CompanyDto;
using LBT_Api.Models.ContactInfoDto;
using LBT_Api.Models.WorkerDto;
using LBT_Api.Other;


namespace LBT_Api.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly LBT_DbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICompanyService _companyService;

        public WorkerService(LBT_DbContext dbContext, IMapper mapper, ICompanyService companyService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _companyService = companyService;
        }

        public GetWorkerDto Create(CreateWorkerDto dto)
        {
            // Check dto
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException();

            Worker worker = _mapper.Map<Worker>(dto);
            try
            {
                _dbContext.Workers.Add(worker);
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }

            GetWorkerDto outputDto = _mapper.Map<GetWorkerDto>(worker);

            return outputDto;
        }

        public void CreateExampleData(int amount)
        {
            for (int i = 0; i < amount; i++)
                CreateData();
        }

        private void CreateData()
        {
            CreateWorkerWithDependenciesDto dto = new CreateWorkerWithDependenciesDto
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
            };// Worker

            CreateWithDependencies(dto);
        }

        public GetWorkerWithDependenciesDto CreateWithDependencies(CreateWorkerWithDependenciesDto dto)
        {
            // Check dto
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException();

            Worker worker = null;

            try
            {
                // Dependencies
                int companyId = _companyService.CreateWithDependencies(dto.Comapny).Id;

                Address address = _mapper.Map<Address>(dto.Address);
                _dbContext.Addresses.Add(address);
                _dbContext.SaveChanges();

                ContactInfo contactInfo = _mapper.Map<ContactInfo>(dto.ContactInfo);
                _dbContext.ContactInfos.Add(contactInfo);
                _dbContext.SaveChanges();

                worker = new Worker
                {
                    Name = dto.Name,
                    Surname = dto.Surname,
                    CompanyId = companyId,
                    AddressId = address.Id,
                    ContactInfoId = contactInfo.Id
                };

                _dbContext.Workers.Add(worker);
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }
            

            // Return dto
            GetWorkerWithDependenciesDto outputDto = _mapper.Map<GetWorkerWithDependenciesDto>(worker);

            return outputDto;
        }

        public void Delete(int id)
        {
            // Check if record exists
            Worker? worker = _dbContext.Workers.FirstOrDefault(w => w.Id == id);
            if (worker == null)
                throw new NotFoundException("Worker with Id: " + id);

            // Delete record
            try
            {
                _dbContext.Workers.Remove(worker);
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }
        }

        public GetWorkerDto Read(int id)
        {
            // Check if record exists
            Worker? worker = _dbContext.Workers.FirstOrDefault(w => w.Id == id);
            if (worker == null)
                throw new NotFoundException("Worker with Id: " + id);

            // Return dto
            GetWorkerDto outputDto = _mapper.Map<GetWorkerDto>(worker);

            return outputDto;
        }

        public GetWorkerWithDependenciesDto ReadWithDependencies(int id)
        {
            // Check if record exists
            Worker? worker = _dbContext.Workers.FirstOrDefault(w => w.Id == id);
            if (worker == null)
                throw new NotFoundException("Worker with Id: " + id);

            // Return dto
            GetWorkerWithDependenciesDto outputDto = _mapper.Map<GetWorkerWithDependenciesDto>(worker);

            return outputDto;
        }

        public GetWorkerDto[] ReadAll()
        {
            Worker[] workers = _dbContext.Workers.ToArray();
            GetWorkerDto[] outputDto = _mapper.Map<GetWorkerDto[]>(workers);

            return outputDto;
        }

        public GetWorkerWithDependenciesDto[] ReadAllWithDependencies()
        {
            Worker[] workers = _dbContext.Workers.ToArray();
            GetWorkerWithDependenciesDto[] outputDto = _mapper.Map<GetWorkerWithDependenciesDto[]>(workers);

            return outputDto;
        }

        public GetWorkerDto Update(UpdateWorkerDto dto)
        {
            // Check dto
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException();

            // Check if record exists
            Worker? worker = _dbContext.Workers.FirstOrDefault(w => w.Id == dto.Id);
            if (worker == null)
                throw new NotFoundException("Worker with Id: " + dto.Id);

            worker.Name = dto.Name != null
                ? dto.Name
                : worker.Name;
            worker.Surname = dto.Surname != null
                ? dto.Surname
                : worker.Surname;

            // Save changes
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new DatabaseOperationFailedException(exception.Message);
            }

            GetWorkerDto outputDto = _mapper.Map<GetWorkerDto>(worker);

            return outputDto;
        }

        public int[] GetAllIds()
        {
            var ids = _dbContext.Workers.AsQueryable().Select(a => a.Id).ToArray();
            return ids;
        }

        private Worker GetRandomWorker()
        {
            int[] items = _dbContext.Workers.AsQueryable().Select(x => x.Id).ToArray();
            Random rnd = new Random();
            int randomIndex = rnd.Next(0, items.Length - 1);

            return _dbContext.Workers.FirstOrDefault(x => x.Id == items[randomIndex])!;
        }

        public void DeleteRandom()
        {
            var item = GetRandomWorker();

            try
            {
                Delete(item.Id);
            }
            catch
            {
                return;
            }
        }

        public void UpdateRandom()
        {
            var item = GetRandomWorker();
            item.Name += "Updated";
            item.Surname += "Updated";

            UpdateWorkerDto dto = new UpdateWorkerDto();

            try
            {
                Update(dto);
            }
            catch
            {
                return;
            }
        }
    }
}
