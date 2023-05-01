using LBT_Api.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LBT_Api.Controllers
{
    [ApiController]
    [Route("api/v1/testing")]
    public class TestingController : ControllerBase
    {
        private readonly ITestingService _testingService;

        public TestingController(ITestingService testingService)
        {
            _testingService = testingService;
        }

        [HttpDelete]
        public ActionResult ClearAll()
        {
            _testingService.ClearAll();
            return Ok();
        }
    }
}
