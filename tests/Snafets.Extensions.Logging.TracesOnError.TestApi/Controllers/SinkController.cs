using Microsoft.AspNetCore.Mvc;

namespace Snafets.Extensions.Logging.TracesOnError.TestApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SinkController(ILogger<SinkController> logger) : ControllerBase
{
    [HttpGet("Exception")]
    public void Get()
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