using AutoMapper;
using LBT_Api.Entities;
using LBT_Api.Exceptions;
using LBT_Api.Interfaces.Services;
using LBT_Api.Models.WorkerDto;
using LBT_Api.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBT_Api.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly LBT_DbContext _dbContext;
        private readonly IMapper _mapper;

        public WorkerService(LBT_DbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public GetWorkerDto Create(CreateWorkerDto dto)
        {
            // Check dto
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Check dto fields
            bool dtoIsValid = Tools.AllStringPropsAreNotNull(dto);
            if (dtoIsValid == false)
                throw new BadRequestException("Dto is missing fields");

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

        public GetWorkerDto[] ReadAll()
        {
            Worker[] workers = _dbContext.Workers.ToArray();
            GetWorkerDto[] workerDtos = _mapper.Map<GetWorkerDto[]>(workers);

            return workerDtos;
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
