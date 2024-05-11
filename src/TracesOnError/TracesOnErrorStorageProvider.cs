using System.Diagnostics;

namespace Snafets.Extensions.Logging.TracesOnError;

internal class TracesOnErrorStorageProvider : ITracesOnErrorStorageProvider
{
    internal const string TracesOnErrorKey = "__TracesOnError__";

    public IList<LogEntry> GetLogs()
    {
        var logsObj = Activity.Current?.GetCustomProperty(TracesOnErrorKey);
        if (logsObj is IList<LogEntry> logs)
            return logs;

        logs = new List<LogEntry>();
        Activity.Current?.SetCustomProperty(TracesOnErrorKey, logs);

        return logs;
    }
}