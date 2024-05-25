using Microsoft.Extensions.Logging;

namespace Snafets.Extensions.Logging.TracesOnError;

/// <summary>
/// Options for the TracesOnError Logger.
/// </summary>
public class TracesOnErrorOptions
{
    internal const string SectionName = "Logging:TracesOnError";

    /// <summary>
    /// Gets or sets the minimum level of log that is required to be activated.
    /// </summary>
    public LogLevel ErrorThreshold { get; set; } = Microsoft.Extensions.Logging.LogLevel.Error;

    /// <summary>
    /// LogLevels for different categories.
    /// </summary>
    public Dictionary<string, string?> LogLevel { get; set; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether the Scope information is included from telemetry or not.
    /// Defaults to true.
    /// </summary>
    public bool IncludeScopes { get; set; } = false;
}