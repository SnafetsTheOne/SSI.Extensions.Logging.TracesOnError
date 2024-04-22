namespace SSI.Extensions.Logging.TracesOnError
{
    internal class FormattedTracesOnErrorLogSink : ITracesOnErrorLogSink
    {
        private readonly ITracesOnErrorFormatter _formatter;
        private readonly Action<string> _sink;

        public FormattedTracesOnErrorLogSink(ITracesOnErrorFormatter formatter, Action<string> sink)
        {
            _formatter = formatter;
            _sink = sink;
        }

        public void WriteLog(IList<LogEntry> logs)
        {
            _sink(_formatter.Process(logs));
        }
    }
}