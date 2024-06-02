using System.Diagnostics;

namespace Snafets.Extensions.Logging.TracesOnError;

internal class DefaultTracesOnErrorStorageProvider : ITracesOnErrorStorageProvider
{
    internal const string TracesOnErrorKey = "__TracesOnError__";

    public void AddLog(LogEntry logEntry)
    {
        var logsObj = Activity.Current?.GetCustomProperty(TracesOnErrorKey);
        if (logsObj is not List<LogEntry> logs)
        {
            logs = new List<LogEntry>();
            Activity.Current?.SetCustomProperty(TracesOnErrorKey, logs);
        }
        logs.Add(logEntry);
    }

    public IReadOnlyList<LogEntry> GetLogs()
    {
        var logsObj = Activity.Current?.GetCustomProperty(TracesOnErrorKey);
        if (logsObj is IReadOnlyList<LogEntry> logs)
            return logs;

        logs = new List<LogEntry>();
        Activity.Current?.SetCustomProperty(TracesOnErrorKey, logs);

        return logs;
    }
}
