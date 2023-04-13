using LBT_Api.Interfaces.Services;
using LBT_Api.Models.ProductDto;
using Microsoft.AspNetCore.Mvc;

namespace LBT_Api.Controllers
{
    [ApiController]
    [Route("api/v1/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public ActionResult<GetProductDto> Create([FromBody] CreateProductDto dto)
        {
            var result = _productService.Create(dto);
            return Created(result.Id.ToString(), result);
        }

        [HttpPost("full")]
        public ActionResult<GetProductWithDependenciesDto> CreateWithDependencies([FromBody] CreateProductWithDependenciesDto dto)
        {
            var result = _productService.CreateWithDependencies(dto);
            return Created(result.Id.ToString(), result);
        }

        [HttpDelete("{id}")]
        public ActionResult Delet([FromRoute] int id)
        {
            _productService.Delete(id);
            return Ok(id);
        }

        [HttpGet]
        public ActionResult<GetProductDto[]> ReadAll()
        {
            var result = _productService.ReadAll();
            return Ok(result);
        }

        [HttpGet("full")]
        public ActionResult<GetProductWithDependenciesDto[]> ReadAllWithDependencies()
        {
            var result = _productService.ReadAllWithDependencies();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<GetProductDto> Read([FromRoute] int id)
        {
            var result = _productService.Read(id);
            return Ok(result);
        }

        [HttpGet("full/{id}")]
        public ActionResult<GetProductWithDependenciesDto> ReadWithDependencies([FromRoute] int id)
        {
            var result = _productService.ReadWithDependencies(id);
            return Ok(result);
        }

        [HttpPatch]
        public ActionResult<GetProductDto> Update([FromBody] UpdateProductDto dto)
        {
            var result = _productService.Update(dto);
            return Ok(result);
        }

    }
}
