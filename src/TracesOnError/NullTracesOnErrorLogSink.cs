namespace Snafets.Extensions.Logging.TracesOnError;

/// <summary>
/// Null object pattern for <see cref="ITracesOnErrorLogSink"/>.
/// </summary>
public class NullTracesOnErrorLogSink : ITracesOnErrorLogSink
{
    public static NullTracesOnErrorLogSink Instance { get; } = new NullTracesOnErrorLogSink();

    private NullTracesOnErrorLogSink()
    {
    }

    /// <inheritdoc />
    public void WriteLog(IList<LogEntry> logs)
    {
    }
}