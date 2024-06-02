using Microsoft.Extensions.Logging;

namespace Snafets.Extensions.Logging.TracesOnError;

/// <summary>
/// Options for the TracesOnError Logger.
/// </summary>
public class TracesOnErrorOptions
{
    /// <summary>
    /// Gets or sets the minimum level of log that is required to be activated.
    /// </summary>
    public LogLevel ErrorThreshold { get; set; } = LogLevel.Error;

    /// <summary>
    /// Gets or sets a for the minimum <see cref="LogLevel"/> to save.
    /// </summary>
    public LogLevel MinimumLevelToInclude { get; set; } = LogLevel.Trace;

    /// <summary>
    /// Explicitly sets the log level for the Microsoft.AspNetCore category.
    /// </summary>
    public LogLevel AspNetCoreLogLevel { get; set; } = LogLevel.Warning;
    
    /// <summary>
    /// Gets or sets a value indicating whether the Scope information is included from telemetry or not.
    /// Defaults to true.
    /// </summary>
    public bool IncludeScopes { get; set; } = false;
}