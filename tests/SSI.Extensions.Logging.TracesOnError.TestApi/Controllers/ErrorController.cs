using Microsoft.AspNetCore.Mvc;

namespace SSI.Extensions.Logging.TracesOnError.TestApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ErrorController(ILogger<ErrorController> logger) : ControllerBase
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
            logger.LogError(ex, "TestController.Get ExceptionHandler");
        }
    }
}