using Microsoft.AspNetCore.Mvc;

namespace SSI.Extensions.Logging.TracesOnError.EnrichExisting.Controllers
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
            catch (Exception ex)
            {
                logger.LogErrorWithTraces(ex, "TestController.Get");
            }
        }
    }
}