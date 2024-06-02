namespace Snafets.Extensions.Logging.TracesOnError;

/// <summary>
/// Stores logs in the context of a web request.
/// </summary>
public interface ITracesOnErrorStorageProvider
{
    /// <summary>
    /// Adds a log entry.
    /// </summary>
    /// <param name="logEntry">the log entry to add.</param>
    void AddLog(LogEntry logEntry);

    /// <summary>
    /// Gets the logs added in the context of a web request.
    /// </summary>
    IReadOnlyList<LogEntry> GetLogs();
}
