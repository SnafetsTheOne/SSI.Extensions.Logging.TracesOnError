namespace SSI.Extensions.Logging.TracesOnError.Tests.Helper
{
    internal class TestStorage : ITracesOnErrorStorageProvider
    {
        public IList<LogEntry> Logs { get; set; } = new List<LogEntry>();

        public IList<LogEntry> GetLogs()
        {
            return Logs;
        }
    }
}