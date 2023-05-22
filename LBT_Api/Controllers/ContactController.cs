using LBT_Api.Interfaces.Services;
using LBT_Api.Models.ContactInfoDto;
using LBT_Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LBT_Api.Controllers
{
    [ApiController]
    [Route("api/v1/contactInfo")]
    public class ContactInfoController : ControllerBase
    {
        private readonly IContactInfoService _contactInfoService;

        public ContactInfoController(IContactInfoService contactInfo)
        {
            _contactInfoService = contactInfo;
        }

        [HttpPost]
        public ActionResult<GetContactInfoDto> Create([FromBody] CreateContactInfoDto dto)
        {
            var result = _contactInfoService.Create(dto);
            return Created(result.Id.ToString(), result);
        }

        [HttpPost("seed/{amount}")]
        public ActionResult Seed([FromRoute] int amount)
        {
            _contactInfoService.CreateExampleData(amount);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _contactInfoService.Delete(id);
            return Ok(id);
        }

        [HttpGet]
        public ActionResult<GetContactInfoDto[]> ReadAll() 
        {
            var result = _contactInfoService.ReadAll();
            return Ok(result);
        }

        [HttpGet("get/ids")]
        public ActionResult<int[]> GetAllIds()
        {
            var result = _contactInfoService.GetAllIds();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<GetContactInfoDto> Read([FromRoute] int id)
        {
            var result = _contactInfoService.Read(id);
            return Ok(result);
        }

        [HttpPatch]
        public ActionResult<GetContactInfoDto> Update([FromBody] UpdateContactInfoDto dto)
        {
            var result = _contactInfoService.Update(dto);
            return Ok(result);
        }

        [HttpDelete("testing/random")]
        public ActionResult DeleteRandom()
        {
            _contactInfoService.DeleteRandom();
            return Ok();
        }

        [HttpPatch("testing/random")]
        public ActionResult UpdateRandom()
        {
            _contactInfoService.UpdateRandom();
            return Ok();
        }
    }
}
