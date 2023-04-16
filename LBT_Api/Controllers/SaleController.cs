using LBT_Api.Interfaces.Services;
using LBT_Api.Models.SaleDto;
using Microsoft.AspNetCore.Mvc;

namespace LBT_Api.Controllers
{
    [ApiController]
    [Route("api/v1/sale")]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpPost]
        public ActionResult<GetSaleDto> Create([FromBody] CreateSaleDto dto)
        {
            var result = _saleService.Create(dto);
            return Created(result.Id.ToString(), result);
        }

        [HttpPost("full")]
        public ActionResult<GetSaleWithDependenciesDto> CreateWithDependencies([FromBody] CreateSaleWithDependenciesDto dto)
        {
            var result = _saleService.CreateWithDependencies(dto);
            return Created(result.Id.ToString(), result);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var result = _saleService.Delete(id);
            return Ok(result);
        }

        [HttpGet]
        public ActionResult<GetSaleDto[]> ReadAll()
        {
            var result = _saleService.ReadAll();
            return Ok(result);
        }

        [HttpGet("full")]
        public ActionResult<GetSaleWithDependenciesDto[]> GetAllWithDependencies()
        {
            var result = _saleService.ReadAllWithDependencies();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<GetSaleDto> Read([FromRoute] int id)
        {
            var result = _saleService.Read(id);
            return Ok(result);
        }

        [HttpGet("full/{id}")]
        public ActionResult<GetSaleWithDependenciesDto> ReadWithDependencies([FromRoute] int id)
        {
            var result = _saleService.ReadWithDependencies(id);
            return Ok(result);
        }

        [HttpPatch]
        public ActionResult<GetSaleDto> Update([FromBody] UpdateSaleDto dto)
        {
            var result = _saleService.Update(dto);
            return Ok(result);
        }
    }
}
