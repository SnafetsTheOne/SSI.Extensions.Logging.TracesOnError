namespace Snafets.Extensions.Logging.TracesOnError.IntegrationTests.Helpers
{
    public class CustomSink : ITracesOnErrorLogSink
    {
        public List<IReadOnlyList<LogEntry>> Logs { get; } = new();

        public void WriteLog(IReadOnlyList<LogEntry> logs)
        {
            Logs.Add(logs);
        }
    }
}
