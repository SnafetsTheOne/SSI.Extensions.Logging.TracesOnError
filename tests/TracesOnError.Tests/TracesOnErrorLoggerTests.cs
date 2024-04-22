using FluentAssertions;
using Microsoft.Extensions.Logging;
using SSI.Extensions.Logging.TracesOnError.Tests.Helper;

namespace SSI.Extensions.Logging.TracesOnError.Tests
{
    public class TracesOnErrorLoggerTests
    {
        private const string _loggerName = "TestLogger";

        [Fact]
        public void Log_Information_EmptySink()
        {
            var sink = new TestSink();
            var storage = new TestStorage();
            var options = new TracesOnErrorOptions();
            var formatter = new TestFormatter();
            var tracesOnErrorLogger = new TracesOnErrorLogger(_loggerName, sink, storage, formatter, null, options);

            tracesOnErrorLogger.LogInformation("Test Message");

            sink.Logs.Should().BeNullOrEmpty();
            storage.GetLogs().Should().HaveCount(1);
        }

        [Fact]
        public void Log_Error_SinkWasWrittenTo()
        {
            var sink = new TestSink();
            var storage = new TestStorage();
            var options = new TracesOnErrorOptions();
            var formatter = new TestFormatter();
            var tracesOnErrorLogger = new TracesOnErrorLogger(_loggerName, sink, storage, formatter, null, options);

            tracesOnErrorLogger.LogError("Test Message");

            sink.Logs.Should().HaveCount(1);
            storage.GetLogs().Should().HaveCount(1);
        }

        [Fact]
        public void Log_InformationThenError_SinkHasAllInfo()
        {
            var sink = new TestSink();
            var storage = new TestStorage();
            var options = new TracesOnErrorOptions();
            var formatter = new TestFormatter();
            var tracesOnErrorLogger = new TracesOnErrorLogger(_loggerName, sink, storage, formatter, null, options);

            tracesOnErrorLogger.LogInformation("Test Message");
            tracesOnErrorLogger.LogError("Test Message");

            sink.Logs.Should().HaveCount(2);
            storage.GetLogs().Should().HaveCount(2);
        }

        [Fact]
        public void Log_ErrorThenInformation_SinkHasErrorOnly()
        {
            var sink = new TestSink();
            var storage = new TestStorage();
            var options = new TracesOnErrorOptions();
            var formatter = new TestFormatter();
            var tracesOnErrorLogger = new TracesOnErrorLogger(_loggerName, sink, storage, formatter, null, options);

            tracesOnErrorLogger.LogError("Test Message");
            tracesOnErrorLogger.LogInformation("Test Message");

            sink.Logs.Should().HaveCount(1);
            storage.GetLogs().Should().HaveCount(2);
        }
    }
}