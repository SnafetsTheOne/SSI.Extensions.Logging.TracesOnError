using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Snafets.Extensions.Logging.TracesOnError.ApplicationInsights;

/// <summary>
/// Extensions for the <see cref="ILoggingBuilder"/> to add TracesOnError with Application Insights.
/// </summary>
public static class TracesOnErrorApplicationInsightsApplicationBuilderExtensions
{
    /// <summary>
    /// Adds TracesOnError logger to the logging pipeline, that logs to Application Insights.
    /// </summary>
    /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
    public static ILoggingBuilder AddTracesOnErrorApplicationInsights(this ILoggingBuilder builder)
    {
        return builder.AddTracesOnErrorApplicationInsights(configure => { }
            , configure => { }, configure => { });
    }

    /// <summary>
    /// Adds TracesOnError logger to the logging pipeline, that logs to Application Insights.
    /// </summary>
    /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
    /// <param name="configureTelemetryConfiguration">Action to configure the telemetry configuration</param>
    public static ILoggingBuilder AddTracesOnErrorApplicationInsights(this ILoggingBuilder builder
        , Action<TelemetryConfiguration> configureTelemetryConfiguration)
    {
        return builder.AddTracesOnErrorApplicationInsights(configureTelemetryConfiguration
            , configure => { }, configure => { });
    }

    /// <summary>
    /// Adds TracesOnError logger to the logging pipeline, that logs to Application Insights.
    /// </summary>
    /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
    /// <param name="configureTelemetryConfiguration">Action to configure the telemetry configuration</param>
    /// <param name="configureTracesOnErrorOptions">Action to configure the logger options</param>
    public static ILoggingBuilder AddTracesOnErrorApplicationInsights(this ILoggingBuilder builder
        , Action<TelemetryConfiguration> configureTelemetryConfiguration
        , Action<TracesOnErrorOptions> configureTracesOnErrorOptions)
    {
        return builder.AddTracesOnErrorApplicationInsights(configureTelemetryConfiguration
            , configureTracesOnErrorOptions, configure => { });
    }

    /// <summary>
    /// Adds TracesOnError logger to the logging pipeline, that logs to Application Insights.
    /// </summary>
    /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
    /// <param name="configureTelemetryConfiguration">Action to configure the telemetry configuration</param>
    /// <param name="configureTracesOnErrorOptions">Action to configure the logger options</param>
    /// <param name="configureOptions">Action to configure the telemetry</param>
    public static ILoggingBuilder AddTracesOnErrorApplicationInsights(this ILoggingBuilder builder
        , Action<TelemetryConfiguration> configureTelemetryConfiguration
        , Action<TracesOnErrorOptions> configureTracesOnErrorOptions
        , Action<TracesOnErrorApplicationInsightsOptions> configureOptions)
    {
        builder.Services.Configure(configureTelemetryConfiguration);
        builder.Services.Configure(configureOptions);

        return builder.AddTracesOnError<ApplicationInsightsLogSink>(configureTracesOnErrorOptions);
    }
}
