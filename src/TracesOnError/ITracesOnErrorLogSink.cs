namespace SSI.Extensions.Logging.TracesOnError
{
    public interface ITracesOnErrorLogSink
    {
        void WriteLog(IList<LogEntry> logs);
    }
}