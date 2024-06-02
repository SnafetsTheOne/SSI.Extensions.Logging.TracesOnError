namespace Snafets.Extensions.Logging.TracesOnError;

/// <summary>
/// Null object pattern for <see cref="ITracesOnErrorLogSink"/>.
/// </summary>
public class NullTracesOnErrorLogSink : ITracesOnErrorLogSink
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NullTracesOnErrorLogSink"/> class.
    /// </summary>
    public NullTracesOnErrorLogSink()
    {
    }

    /// <inheritdoc />
    public void WriteLog(IReadOnlyList<LogEntry> logs)
    {
    }
}
