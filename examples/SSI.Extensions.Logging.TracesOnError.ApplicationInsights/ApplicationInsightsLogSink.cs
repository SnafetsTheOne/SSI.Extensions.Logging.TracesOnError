namespace SSI.Extensions.Logging.TracesOnError.ApplicationInsights
{
    public class ApplicationInsightsLogSink : ITracesOnErrorLogSink
    {
        public void WriteLog(IList<LogEntry> logs)
        {
            throw new NotImplementedException();
        }
    }
}
