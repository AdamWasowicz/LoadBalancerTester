﻿using LBT_Api.Interfaces.Services;
using LBT_Api.Models.WorkerDto;
using LBT_Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LBT_Api.Controllers
{
    [ApiController]
    [Route("api/v1/worker")]
    public class WorkerController : ControllerBase
    {
        private readonly IWorkerService _workerService;

        public WorkerController(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        [HttpPost]
        public ActionResult<GetWorkerDto> Create([FromBody] CreateWorkerDto dto)
        {
            var result = _workerService.Create(dto);
            return Created(result.Id.ToString(), result);
        }

        [HttpPost("seed/{amount}")]
        public ActionResult Seed([FromRoute] int amount)
        {
            _workerService.CreateExampleData(amount);
            return Ok();
        }

        [HttpPost("full")]
        public ActionResult<GetWorkerWithDependenciesDto> CreateWithDependencies([FromBody] CreateWorkerWithDependenciesDto dto)
        {
            var result = _workerService.CreateWithDependencies(dto);
            return Created(result.Id.ToString(), result);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _workerService.Delete(id);
            return Ok(id);
        }

        [HttpGet]
        public ActionResult<GetWorkerDto[]> ReadAll()
        {
            var result = _workerService.ReadAll();
            return Ok(result);
        }

        [HttpGet("full")]
        public ActionResult<GetWorkerWithDependenciesDto[]> ReadAllWithDependencies()
        {
            var result = _workerService.ReadAllWithDependencies();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<GetWorkerDto> Read([FromRoute] int id) 
        {
            var result = _workerService.Read(id);
            return Ok(result);
        }

        [HttpGet("get/ids")]
        public ActionResult<int[]> GetAllIds()
        {
            var result = _workerService.GetAllIds();
            return Ok(result);
        }

        [HttpGet("full/{id}")]
        public ActionResult<GetWorkerWithDependenciesDto> ReadWithDependencies([FromRoute] int id)
        {
            var result = _workerService.ReadWithDependencies(id);
            return Ok(result);
        }

        [HttpPatch]
        public ActionResult<GetWorkerDto> Update([FromBody] UpdateWorkerDto dto)
        {
            var result = _workerService.Update(dto);
            return Ok(result);
        }

        [HttpDelete("testing/random")]
        public ActionResult DeleteRandom()
        {
            _workerService.DeleteRandom();
            return Ok();
        }

        [HttpPatch("testing/random")]
        public ActionResult UpdateRandom()
        {
            _workerService.UpdateRandom();
            return Ok();
        }
    }
}
