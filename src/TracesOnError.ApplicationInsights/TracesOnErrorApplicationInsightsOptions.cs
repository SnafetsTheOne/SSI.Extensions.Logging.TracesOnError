using Microsoft.ApplicationInsights.DataContracts;

namespace Snafets.Extensions.Logging.TracesOnError.ApplicationInsights;

/// <summary>
/// Options for the logging of the Application Insights Telemetry.
/// </summary>
public class TracesOnErrorApplicationInsightsOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether to track exceptions as <see cref="ExceptionTelemetry"/>.
    /// Defaults to true.
    /// </summary>
    public bool TrackExceptionsAsExceptionTelemetry { get; set; } = true;
    /// <summary>
    /// Whether to include the log level in the telemetry.
    /// Defaults to true.
    /// </summary>
    public bool IncludeLogLevel { get; set; } = true;
    /// <summary>
    /// Whether to include the category in the telemetry.
    /// Defaults to true.
    /// </summary>
    public bool IncludeCategory { get; set; } = true;
    /// <summary>
    /// Whether to include the event id in the telemetry.
    /// Defaults to true.
    /// </summary>
    public bool IncludeEventId { get; set; } = true;
}

