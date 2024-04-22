using System.Diagnostics;

namespace SSI.Extensions.Logging.TracesOnError
{
    internal class TracesOnErrorStorageProvider : ITracesOnErrorStorageProvider
    {
        internal const string TracesOnErrorKey = "__TracesOnError__";

        public TracesOnErrorStorageProvider()
        {
        }

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
}