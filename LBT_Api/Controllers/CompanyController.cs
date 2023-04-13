using LBT_Api.Interfaces.Services;
using LBT_Api.Models.CompanyDto;
using Microsoft.AspNetCore.Mvc;

namespace LBT_Api.Controllers
{
    [ApiController]
    [Route("api/v1/company")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost]
        public ActionResult<GetCompanyDto> Create([FromBody] CreateCompanyDto dto)
        {
            var result = _companyService.Create(dto);
            return Created(result.Id.ToString(), result);
        }

        [HttpPost("full")]
        public ActionResult<GetCompanyWithDependenciesDto> CreateWithDependencies([FromBody] CreateCompanyWithDependenciesDto dto)
        {
            var result = _companyService.CreateWithDependencies(dto);
            return Created(result.Id.ToString(), result);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _companyService.Delete(id);
            return Ok(id);
        }

        [HttpGet]
        public ActionResult<GetCompanyDto[]> ReadAll()
        {
            var result = _companyService.ReadAll();
            return Ok(result);
        }

        [HttpGet("full")]
        public ActionResult<GetCompanyWithDependenciesDto[]> ReadAllWithDependencies()
        {
            var result = _companyService.ReadAllWithDependencies();
            return Ok(result);
        }


        [HttpGet("{id}")]
        public ActionResult<GetCompanyDto> Read([FromRoute] int id)
        {
            var result = _companyService.Read(id);
            return Ok(result);
        }

        [HttpGet("full/{id}")]
        public ActionResult<GetCompanyWithDependenciesDto> ReadWithDependencies([FromRoute] int id)
        {
            var result = _companyService.ReadWithDependencies(id);
            return Ok(result);
        }

        [HttpPatch("name")]
        public ActionResult<GetCompanyDto> UpdateName([FromBody] UpdateCompanyNameDto dto)
        {
            var result = _companyService.UpdateName(dto);
            return Ok(result);
        }
    }
}
