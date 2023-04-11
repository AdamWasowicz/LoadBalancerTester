using LBT_Api.Interfaces.Services;
using LBT_Api.Models.ProductSoldDto;
using Microsoft.AspNetCore.Mvc;

namespace LBT_Api.Controllers
{
    [ApiController]
    [Route("api/v1/productSold")]
    public class ProductSoldController : ControllerBase
    {
        private readonly IProductSoldService _productSoldService;

        public ProductSoldController(IProductSoldService productSoldService)
        {
            _productSoldService = productSoldService;
        }

        [HttpPost]
        public ActionResult<GetProductSoldDto> Create([FromBody] CreateProductSoldDto dto)
        {
            var result = _productSoldService.Create(dto);
            return Created(result.Id.ToString(), result);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var result = _productSoldService.Delete(id);
            return Ok(result);
        }

        [HttpGet]
        public ActionResult<GetProductSoldDto[]> ReadAll()
        {
            var result = _productSoldService.ReadAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<GetProductSoldDto> Read([FromRoute] int id)
        {
            var result = _productSoldService.Read(id);
            return Ok(result);
        }

        [HttpPatch]
        public ActionResult<GetProductSoldDto> Update([FromBody] UpdateProductSoldDto dto)
        {
            var result = _productSoldService.Update(dto);
            return Ok(result);
        }
    }
}
