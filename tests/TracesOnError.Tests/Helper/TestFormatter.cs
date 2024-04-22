namespace SSI.Extensions.Logging.TracesOnError.Tests.Helper
{
    internal class TestFormatter : ITracesOnErrorFormatter
    {
        public string Process(IList<LogEntry> logs)
        {
            throw new NotImplementedException();
        }
    }
}
