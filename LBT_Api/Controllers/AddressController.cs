using LBT_Api.Interfaces.Services;
using LBT_Api.Models.AddressDto;
using Microsoft.AspNetCore.Mvc;

namespace LBT_Api.Controllers
{
    [ApiController]
    [Route("api/v1/adress")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpPost]
        public ActionResult<GetAddressDto> Create([FromBody] CreateAddressDto dto)
        {
            var result = _addressService.Create(dto);
            return Created(result.Id.ToString(), result);
        }

        [HttpPost("seed/{amount}")]
        public ActionResult Seed([FromRoute] int amount)
        {
            _addressService.CreateExampleData(amount);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _addressService.Delete(id);
            return Ok(id);
        }

        [HttpGet]
        public ActionResult<GetAddressDto[]> ReadAll()
        {
            var result = _addressService.ReadAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<GetAddressDto> Read([FromRoute] int id)
        {
            var result = _addressService.Read(id);
            return Ok(result);
        }

        [HttpPatch]
        public ActionResult<GetAddressDto> Update([FromBody] UpdateAddressDto dto)
        {
            var result = _addressService.Update(dto);
            return Ok(result);
        }
    }
}
