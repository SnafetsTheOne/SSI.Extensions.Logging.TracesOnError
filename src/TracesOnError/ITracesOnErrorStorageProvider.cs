namespace Snafets.Extensions.Logging.TracesOnError;

internal interface ITracesOnErrorStorageProvider
{
    IList<LogEntry> GetLogs();
}