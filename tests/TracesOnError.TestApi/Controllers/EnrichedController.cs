using Microsoft.AspNetCore.Mvc;

namespace Snafets.Extensions.Logging.TracesOnError.TestApi.Controllers;

[ApiController]
[Route("[controller]")]

public class EnrichedController(ILogger<EnrichedController> logger) : ControllerBase
{
    const string TraceMessage = "TraceMessage";

    [HttpGet("Error/Message")]
    public void ErrorMessage()
    {
        logger.LogTrace(TraceMessage);

        logger.LogErrorWithTraces("ErrorMessage");
    }

    [HttpGet("Error/Exception")]
    public void ErrorException()
    {
        logger.LogTrace(TraceMessage);

        try
        {
            throw new Exception("ExceptionMessage");
        }
        catch (Exception ex)
        {
            logger.LogErrorWithTraces(ex, "ErrorMessage");
        }
    }
    [HttpGet("Critical/Message")]
    public void CriticalMessage()
    {
        logger.LogTrace(TraceMessage);

        logger.LogCriticalWithTraces("CriticalMessage");
    }

    [HttpGet("Critical/Exception")]
    public void CriticalException()
    {
        logger.LogTrace(TraceMessage);

        try
        {
            throw new Exception("ExceptionMessage");
        }
        catch (Exception ex)
        {
            logger.LogCriticalWithTraces(ex, "CriticalMessage");
        }
    }
}

