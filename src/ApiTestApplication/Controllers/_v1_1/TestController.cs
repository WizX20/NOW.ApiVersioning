using Microsoft.AspNetCore.Mvc;

namespace ApiTestApplication.Controllers.v1_1
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(
            ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet, ResponseCache(CacheProfileName = Constants.ResponseCache.DefaultCacheProfile)]
        public IActionResult Get()
        {
            return Ok(nameof(TestController) + " v1.1");
        }
    }
}