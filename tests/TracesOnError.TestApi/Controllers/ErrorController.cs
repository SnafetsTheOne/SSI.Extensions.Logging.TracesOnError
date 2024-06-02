using Microsoft.AspNetCore.Mvc;

namespace Snafets.Extensions.Logging.TracesOnError.TestApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ErrorController(ILogger<ErrorController> logger) : ControllerBase
{
    [HttpGet("NoError")]
    public void Get()
    {
        logger.LogTrace("TraceMessage");
    }

    [HttpGet("Exception")]
    public void GetException()
    {
        logger.LogTrace("TraceMessage");

        try
        {
            throw new Exception("ExceptionMessage");
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "ErrorMessage");
        }
    }

    [HttpGet("Message")]
    public void GetMessage()
    {
        logger.LogTrace("TraceMessage");

        logger.LogError("ErrorMessage");
    }
}