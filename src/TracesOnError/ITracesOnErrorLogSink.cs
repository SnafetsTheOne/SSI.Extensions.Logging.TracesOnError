namespace Snafets.Extensions.Logging.TracesOnError;

/// <summary>
/// Represents a log sink for TracesOnError.
/// </summary>
public interface ITracesOnErrorLogSink
{
    /// <summary>
    /// Writes the logs to the sink.
    /// </summary>
    void WriteLog(IList<LogEntry> logs);
}