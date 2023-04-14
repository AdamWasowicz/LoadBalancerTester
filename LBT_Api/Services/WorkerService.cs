﻿using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
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

        public GetWorkerWithDependenciesDto CreateWithDependencies(CreateWorkerWithDependenciesDto dto)
        {
            // Check dto
            if (Tools.ModelIsValid(dto) == false)
                throw new InvalidModelException();

            var transaction = _dbContext.Database.BeginTransaction();
            Worker worker = null;

            try
            {
                // Dependencies
                int companyId = _companyService.CreateWithDependencies(dto.Comapny).Id;

                Address address = _mapper.Map<Address>(dto.Address);
                _dbContext.Addresses.Add(address);

                ContactInfo contactInfo = _mapper.Map<ContactInfo>(dto.ContactInfo);
                _dbContext.ContactInfos.Add(contactInfo);

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
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw new DatabaseOperationFailedException(exception.Message);
            }

            // Return dto
            GetWorkerWithDependenciesDto outputDto = _mapper.Map<GetWorkerWithDependenciesDto>(worker);

            return outputDto;
        }

        public int Delete(int id)
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

            return 0;
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
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.Id == null)
                throw new BadRequestException("Dto is missing Id field");

            // Check if record exists
            Worker? worker = _dbContext.Workers.FirstOrDefault(w => w.Id == dto.Id);
            if (worker == null)
                throw new NotFoundException("Worker with Id: " + dto.Id);

            Worker mappedFromDto = _mapper.Map<Worker>(dto);
            worker = Tools.UpdateObjectProperties(worker, mappedFromDto);

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
    }
}
