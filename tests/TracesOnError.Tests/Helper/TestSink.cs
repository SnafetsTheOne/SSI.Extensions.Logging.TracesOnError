namespace SSI.Extensions.Logging.TracesOnError.Tests.Helper
{
    internal class TestSink : ITracesOnErrorLogSink
    {
        public IList<LogEntry> Logs { get; set; } = default!;

        public void WriteLog(IList<LogEntry> logs)
        {
            Logs = logs.ToList();
        }
    }
}