using LBT_Api.Interfaces.Services;
using LBT_Api.Models.WorkerDto;
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

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var result = _workerService.Delete(id);
            return Ok(result);
        }

        [HttpGet]
        public ActionResult<GetWorkerDto[]> ReadAll()
        {
            var result = _workerService.ReadAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<GetWorkerDto> Read([FromRoute] int id) 
        {
            var result = _workerService.Read(id);
            return Ok(result);
        }

        [HttpPatch]
        public ActionResult<GetWorkerDto> Update([FromBody] UpdateWorkerDto dto)
        {
            var result = _workerService.Update(dto);
            return Ok(result);
        }
    }
}
