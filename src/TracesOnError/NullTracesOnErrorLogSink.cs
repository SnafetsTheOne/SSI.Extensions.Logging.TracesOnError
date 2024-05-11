namespace Snafets.Extensions.Logging.TracesOnError;

public class NullTracesOnErrorLogSink : ITracesOnErrorLogSink
{
    public static NullTracesOnErrorLogSink Instance { get; } = new NullTracesOnErrorLogSink();

    private NullTracesOnErrorLogSink()
    {
    }

    public void WriteLog(IList<LogEntry> logs)
    {
    }
}