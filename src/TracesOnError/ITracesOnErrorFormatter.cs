namespace Snafets.Extensions.Logging.TracesOnError;

/// <summary>
/// Formatter for TracesOnError Logger.
/// </summary>
public interface ITracesOnErrorFormatter 
{
    /// <summary>
    /// Processes the logs and returns a formatted string.
    /// </summary>
    /// <param name="logs">the logs to format</param>
    /// <returns>the formatted string.</returns>
    string Process(IList<LogEntry> logs);

    /// <summary>
    /// Processes the logs messages and returns a formatted string.
    /// </summary>
    /// <param name="logs">the logs to format</param>
    /// <returns>the formatted string.</returns>
    string ProcessMessagesOnly(IList<LogEntry> logs);

    /// <summary>
    /// Processes the scopes and returns a formatted string.
    /// </summary>
    /// <param name="obj">the scope object</param>
    /// <param name="state">the list of previously formatted scopes of the log entry</param>
    void ScopeCallback(object? obj, IList<string?> state)
    {
        state.Add(obj?.ToString());
    }
}