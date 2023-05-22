using LBT_Api.Interfaces.Services;
using LBT_Api.Models.CompanyDto;
using LBT_Api.Models.SupplierDto;
using LBT_Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LBT_Api.Controllers
{
    [ApiController]
    [Route("api/v1/supplier")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpPost]
        public ActionResult<GetSupplierDto> Create([FromBody] CreateSupplierDto dto)
        {
            var result = _supplierService.Create(dto);
            return Created(result.Id.ToString(), result);
        }

        [HttpPost("seed/{amount}")]
        public ActionResult Seed([FromRoute] int amount)
        {
            _supplierService.CreateExampleData(amount);
            return Ok();
        }

        [HttpPost("full")]
        public ActionResult<GetSupplierWithDependenciesDto> CreateWithDependencies([FromBody] CreateSupplierWithDependenciesDto dto)
        {
            var result = _supplierService.CreateWithDependencies(dto);
            return Created(result.Id.ToString(), result);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _supplierService.Delete(id);
            return Ok(id);
        }

        [HttpGet]
        public ActionResult<GetSupplierDto[]> ReadAll()
        {
            var result = _supplierService.ReadAll();
            return Ok(result);
        }

        [HttpGet("full")]
        public ActionResult<GetCompanyWithDependenciesDto[]> ReadAllWithDependencies()
        {
            var result = _supplierService.ReadAllWithDependencies();
            return Ok(result);
        }

        [HttpGet("get/ids")]
        public ActionResult<int[]> GetAllIds()
        {
            var result = _supplierService.GetAllIds();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<GetSupplierDto> Read([FromRoute] int id)
        {
            var result = _supplierService.Read(id);
            return Ok(result);
        }

        [HttpGet("full/{id}")]
        public ActionResult<GetSupplierWithDependenciesDto> ReadWithDependencies([FromRoute] int id)
        {
            var result = _supplierService.ReadWithDependencies(id);
            return Ok(result);
        }

        [HttpPatch]
        public ActionResult<GetSupplierDto> Update([FromBody] UpdateSupplierDto dto)
        {
            var result = _supplierService.Update(dto);
            return Ok(result);
        }

        [HttpDelete("testing/random")]
        public ActionResult DeleteRandom()
        {
            _supplierService.DeleteRandom();
            return Ok();
        }

        [HttpPatch("testing/random")]
        public ActionResult UpdateRandom()
        {
            _supplierService.UpdateRandom();
            return Ok();
        }
    }
}
