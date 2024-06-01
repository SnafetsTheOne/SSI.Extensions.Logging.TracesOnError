using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Snafets.Extensions.Logging.TracesOnError.ApplicationInsights;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;

namespace Snafets.Extensions.Logging.TracesOnError.ApplicationInsightsTests
{
    public class ApplicationInsightsLogSinkTests
    {
        private readonly TelemetryConfiguration _telemetryConfiguration;
        private readonly TelemetryClient _telemetryClient;
        private readonly IList<ITelemetry> _telemetryItems;

        private static readonly LogEntry LogEntry1 = new LogEntry
        {
            LogLevel = LogLevel.Critical,
            Category = "Category 1",
            EventId = new EventId(1, "1"),
            Message = "Message 1",
            Exception = new Exception("Test exception"),
            Scopes = new List<string?> { "Scope 1.1", "Scope 1.1" }
        };
        private static readonly LogEntry LogEntry2 = new LogEntry
        {
            LogLevel = LogLevel.Information,
            Category = "Category 2",
            EventId = new EventId(2),
            Message = "Message 2",
            Scopes = new List<string?> { "Scope 2" }
        };
        private static readonly LogEntry LogEntry3 = new LogEntry
        {
            LogLevel = LogLevel.Information,
            Category = "Category 3",
            Message = "Message 3"
        };


        public ApplicationInsightsLogSinkTests()
        {
            _telemetryItems = new List<ITelemetry>();
            _telemetryConfiguration = new TelemetryConfiguration();
            _telemetryConfiguration.TelemetryChannel = new StubTelemetryChannel
            {
                OnSend = (telemetry) => _telemetryItems.Add(telemetry)
            };
            _telemetryClient = new TelemetryClient(_telemetryConfiguration);
        }

        [Fact]
        public void WriteLog_SingleLog_CorrectTelemetry()
        {
            var logs = new List<LogEntry>
            {
                LogEntry1
            };
            var sut = new ApplicationInsightsLogSink(_telemetryClient, new TracesOnErrorApplicationInsightsOptions());

            sut.WriteLog(logs);

            _telemetryItems.Should().HaveCount(1);
            var telemetry = _telemetryItems[0];
            telemetry.Should().BeAssignableTo<ExceptionTelemetry>();
        }

        [Fact]
        public void WriteLog_MultipleLogs_CorrectTelemetry()
        {
            var logs = new List<LogEntry>
            {
                LogEntry3,
                LogEntry2,
                LogEntry1,
            };
            var sut = new ApplicationInsightsLogSink(_telemetryClient, new TracesOnErrorApplicationInsightsOptions());

            sut.WriteLog(logs);

            _telemetryItems.Should().HaveCount(1);
            var telemetry = _telemetryItems[0] as ExceptionTelemetry;
            telemetry.Should().NotBeNull();
            telemetry!.Exception.Should().Be(LogEntry1.Exception);
            telemetry.Message.Should().Be(LogEntry1.Message);
            telemetry.SeverityLevel.Should().Be(SeverityLevel.Critical);

            telemetry.Properties.Should().ContainKey("trace_0_CategoryName").WhoseValue.Should().Be(LogEntry3.Category);
            telemetry.Properties.Should().ContainKey("trace_0_Message").WhoseValue.Should().Be(LogEntry3.Message);
            telemetry.Properties.Should().ContainKey("trace_0_LogLevel").WhoseValue.Should().Be(LogEntry3.LogLevel.ToString());
            telemetry.Properties.Should().NotContainKey("trace_0_EventName");
            telemetry.Properties.Should().NotContainKey("trace_0_EventId");
            telemetry.Properties.Should().NotContainKey("trace_0_Exception");
            telemetry.Properties.Should().NotContainKey("trace_0_Scopes");

            telemetry.Properties.Should().ContainKey("trace_1_CategoryName").WhoseValue.Should().Be(LogEntry2.Category);
            telemetry.Properties.Should().ContainKey("trace_1_Message").WhoseValue.Should().Be(LogEntry2.Message);
            telemetry.Properties.Should().ContainKey("trace_1_LogLevel").WhoseValue.Should().Be(LogEntry2.LogLevel.ToString());
            telemetry.Properties.Should().ContainKey("trace_1_EventId").WhoseValue.Should().Be(LogEntry2.EventId.Id.ToString());
            telemetry.Properties.Should().NotContainKey("trace_1_EventName");
            telemetry.Properties.Should().NotContainKey("trace_1_Exception");
            telemetry.Properties.Should().NotContainKey("trace_1_Scopes");

            telemetry.Properties.Should().ContainKey("trace_2_CategoryName").WhoseValue.Should().Be(LogEntry1.Category);
            telemetry.Properties.Should().ContainKey("trace_2_Message").WhoseValue.Should().Be(LogEntry1.Message);
            telemetry.Properties.Should().ContainKey("trace_2_LogLevel").WhoseValue.Should().Be(LogEntry1.LogLevel.ToString());
            telemetry.Properties.Should().ContainKey("trace_2_EventId").WhoseValue.Should().Be(LogEntry1.EventId.Id.ToString());
            telemetry.Properties.Should().ContainKey("trace_2_EventName").WhoseValue.Should().Be(LogEntry1.EventId.Name);
            telemetry.Properties.Should().ContainKey("trace_2_Exception").WhoseValue.Should().Be(LogEntry1.Exception!.ToString());
            telemetry.Properties.Should().NotContainKey("trace_2_Scopes");
        }

        [Fact]
        public void WriteLog_OptionsInverted_AllRespected()
        {
            var logs = new List<LogEntry>
            {
                LogEntry1
            };
            var sut = new ApplicationInsightsLogSink(_telemetryClient, new TracesOnErrorApplicationInsightsOptions()
            {
                IncludeCategory = false,
                IncludeEventId = false,
                IncludeLogLevel = false,
                IncludeScopes = true,
                TrackExceptionsAsExceptionTelemetry = false
            });

            sut.WriteLog(logs);

            _telemetryItems.Should().HaveCount(1);
            var telemetry = _telemetryItems[0] as TraceTelemetry;

            telemetry!.Properties.Should().NotContainKey("trace_0_CategoryName");
            telemetry.Properties.Should().ContainKey("trace_0_Message").WhoseValue.Should().Be(LogEntry1.Message);
            telemetry.Properties.Should().NotContainKey("trace_0_LogLevel");
            telemetry.Properties.Should().NotContainKey("trace_0_EventId");
            telemetry.Properties.Should().NotContainKey("trace_0_EventName");
            telemetry.Properties.Should().ContainKey("trace_0_Exception").WhoseValue.Should().Be(LogEntry1.Exception!.ToString());
            telemetry.Properties.Should().NotContainKey("trace_0_Scopes");
        }

        [Fact]
        public void WriteLogs_EmptyLog_NothingHappens()
        {
            var sut = new ApplicationInsightsLogSink(_telemetryClient, new TracesOnErrorApplicationInsightsOptions());

            sut.WriteLog(new List<LogEntry>());

            _telemetryItems.Should().HaveCount(0);
        }
    }
}