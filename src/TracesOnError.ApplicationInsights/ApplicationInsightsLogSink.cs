using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Microsoft.ApplicationInsights;

namespace Snafets.Extensions.Logging.TracesOnError.ApplicationInsights;

internal class ApplicationInsightsLogSink : ITracesOnErrorLogSink
{
    private readonly TelemetryClient _telemetryClient;
    private readonly TracesOnErrorApplicationInsightsOptions _options;

    public ApplicationInsightsLogSink(TelemetryClient telemetryClient, TracesOnErrorApplicationInsightsOptions options)
    {
        _telemetryClient = telemetryClient;
        _options = options;
    }

    public void WriteLog(IList<LogEntry> logs)
    {
        if (!logs.Any())
            return;

        var triggeringLog = logs.Last();

        if (triggeringLog.Exception != null && _options.TrackExceptionsAsExceptionTelemetry)
        {
            var telemetry = new ExceptionTelemetry()
            {
                Message = triggeringLog.Message,
                Exception = triggeringLog.Exception,
                SeverityLevel = GetSeverityLevel(triggeringLog.LogLevel)
            };
            PopulateTelemetry(telemetry.Properties, logs);
            _telemetryClient.TrackException(telemetry);
        }
        else
        {
            var telemetry = new TraceTelemetry()
            {
                Message = triggeringLog.Message,
                SeverityLevel = GetSeverityLevel(triggeringLog.LogLevel)
            };
            PopulateTelemetry(telemetry.Properties, logs);
            _telemetryClient.TrackTrace(telemetry);
        }
    }

    private static SeverityLevel GetSeverityLevel(LogLevel logLevel)
    {
        switch (logLevel)
        {
            case LogLevel.Critical:
                return SeverityLevel.Critical;
            case LogLevel.Error:
                return SeverityLevel.Error;
            case LogLevel.Warning:
                return SeverityLevel.Warning;
            case LogLevel.Information:
                return SeverityLevel.Information;
            case LogLevel.Debug:
            case LogLevel.Trace:
            default:
                return SeverityLevel.Verbose;
        }
    }

    private void PopulateTelemetry(IDictionary<string, string> telemetryItem, IList<LogEntry> logs)
    {
        for(var i = 0; i < logs.Count; i++)
        {
            PopulateTelemetry(telemetryItem, $"trace_{i}_", logs[i]);
        }
    }

    private void PopulateTelemetry(IDictionary<string, string> dict, string prefix, LogEntry log)
    {
        dict[prefix + "Message"] = log.Message;
        if (_options.IncludeCategory)
        {
            dict[prefix + "CategoryName"] = log.Category;
        }
        if(_options.IncludeLogLevel)
        {
            dict[prefix + "LogLevel"] = log.LogLevel.ToString();
        }
        if (log.Exception != null)
        {
            dict[prefix + "Exception"] = log.Exception.ToString();
        }
        if(_options.IncludeEventId && log.EventId != 0)
        {
            dict[prefix + "EventId"] = log.EventId.Id.ToString(CultureInfo.InvariantCulture);
        }
        if(_options.IncludeEventId && !string.IsNullOrEmpty(log.EventId.Name))
        {
            dict[prefix + "EventName"] = log.EventId.Name;
        }
        if(_options.IncludeScopes)
        {
            dict[prefix + "Scope"] = string.Join(" => ", log.Scopes);
        }
    }
}