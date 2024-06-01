using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;

namespace Snafets.Extensions.Logging.TracesOnError.ApplicationInsights;

/// <summary>
/// Extensions for the <see cref="ILoggingBuilder"/> to add TracesOnError with Application Insights.
/// </summary>
public static class TracesOnErrorApplicationInsightsExtensions
{
    /// <summary>
    /// Adds TracesOnError to the logging pipeline.
    /// Adds a LogSink that logs to Application Insights.
    /// </summary>
    /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
    /// <param name="telemetryClient">The &lt;see cref="TelemetryClient"/&gt; to use.</param>
    /// <param name="configureOptions">Action to configure the options.</param>
    public static void AddTracesOnErrorApplicationInsights(this ILoggingBuilder builder, TelemetryClient telemetryClient
        , Action<TracesOnErrorApplicationInsightsOptions>? configureOptions = null)
    {
        var options = new TracesOnErrorApplicationInsightsOptions();
        configureOptions?.Invoke(options);
        var logSink = new ApplicationInsightsLogSink(telemetryClient, options);

        builder.AddTracesOnError(logSink);
    }
}
