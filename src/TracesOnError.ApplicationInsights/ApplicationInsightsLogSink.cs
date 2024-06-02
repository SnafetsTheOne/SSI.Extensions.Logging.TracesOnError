using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Options;

namespace Snafets.Extensions.Logging.TracesOnError.ApplicationInsights;

internal class ApplicationInsightsLogSink : ITracesOnErrorLogSink
{
    private readonly TelemetryClient _telemetryClient;
    private readonly IOptions<TracesOnErrorOptions> _options;
    private readonly IOptions<TracesOnErrorApplicationInsightsOptions> _aiOptions;

    public ApplicationInsightsLogSink(IOptions<TelemetryConfiguration> telemetryConfiguration
        , IOptions<TracesOnErrorOptions> options
        , IOptions<TracesOnErrorApplicationInsightsOptions> aiOptions)
    {
        _telemetryClient = new TelemetryClient(telemetryConfiguration.Value);
        _options = options;
        _aiOptions = aiOptions;
    }

    public void WriteLog(IReadOnlyList<LogEntry> logs)
    {
        if (!logs.Any())
            return;

        var triggeringLog = logs[^1];

        if (triggeringLog.Exception != null && _aiOptions.Value.TrackExceptionsAsExceptionTelemetry)
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
        return logLevel switch
        {
            LogLevel.Critical => SeverityLevel.Critical,
            LogLevel.Error => SeverityLevel.Error,
            LogLevel.Warning => SeverityLevel.Warning,
            LogLevel.Information => SeverityLevel.Information,
            LogLevel.Debug => SeverityLevel.Verbose,
            LogLevel.Trace => SeverityLevel.Verbose,
            _ => SeverityLevel.Verbose
        };
    }

    private void PopulateTelemetry(IDictionary<string, string> telemetryItem, IReadOnlyList<LogEntry> logs)
    {
        for(var i = 0; i < logs.Count; i++)
        {
            PopulateTelemetry(telemetryItem, $"trace_{i}_", logs[i]);
        }
    }

    private void PopulateTelemetry(IDictionary<string, string> dict, string prefix, LogEntry log)
    {
        dict[prefix + "Message"] = log.Message;
        if (_aiOptions.Value.IncludeCategory)
        {
            dict[prefix + "CategoryName"] = log.Category;
        }
        if(_aiOptions.Value.IncludeLogLevel)
        {
            dict[prefix + "LogLevel"] = log.LogLevel.ToString();
        }
        if (log.Exception != null)
        {
            dict[prefix + "Exception"] = log.Exception.ToString();
        }
        if(_aiOptions.Value.IncludeEventId && log.EventId != 0)
        {
            dict[prefix + "EventId"] = log.EventId.Id.ToString(CultureInfo.InvariantCulture);
        }
        if(_aiOptions.Value.IncludeEventId && !string.IsNullOrEmpty(log.EventId.Name))
        {
            dict[prefix + "EventName"] = log.EventId.Name;
        }
        if(_options.Value.IncludeScopes && log.Scopes != null)
        {
            dict[prefix + "Scope"] = string.Join(" => ", log.Scopes);
        }
    }
}