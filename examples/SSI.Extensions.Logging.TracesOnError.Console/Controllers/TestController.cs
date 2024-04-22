using Microsoft.AspNetCore.Mvc;

namespace SSI.Extensions.Logging.TracesOnError.Console.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController(ILogger<TestController> logger) : ControllerBase
    {
        [HttpGet]
        public void Get()
        {
            logger.LogTrace("TestController.Get");

            try
            {
                throw new Exception("Test exception");
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "TestController.Get");
            }
        }
    }
}
