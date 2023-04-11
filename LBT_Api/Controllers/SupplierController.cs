using LBT_Api.Interfaces.Services;
using LBT_Api.Models.SupplierDto;
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

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var result = _supplierService.Delete(id);
            return Ok(result);
        }

        [HttpGet]
        public ActionResult<GetSupplierDto[]> ReadAll()
        {
            var result = _supplierService.ReadAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<GetSupplierDto> Read([FromRoute] int id)
        {
            var result = _supplierService.Read(id);
            return Ok(result);
        }

        [HttpPatch]
        public ActionResult<GetSupplierDto> Update([FromBody] UpdateSupplierDto dto)
        {
            var result = _supplierService.Update(dto);
            return Ok(result);
        }
    }
}
