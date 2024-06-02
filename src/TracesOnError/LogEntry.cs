using Microsoft.Extensions.Logging;

namespace Snafets.Extensions.Logging.TracesOnError;

internal record LogEntry<TState> : LogEntry
{
    public TState State { get; init; } = default!;
    public Func<TState, Exception?, string> Formatter { get; init; } = default!;
    public override string Message => Formatter(State, Exception);
}

/// <summary>
/// Represents a log entry.
/// </summary>
public record LogEntry
{
    /// <summary>
    /// Gets the log category
    /// </summary>
    public string Category { get; init; } = default!;
    /// <summary>
    /// Gets the LogLevel
    /// </summary>
    public LogLevel LogLevel { get; init; }
    /// <summary>
    /// Gets the log EventId
    /// </summary>
    public EventId EventId { get; init; }
    /// <summary>
    /// Gets the log message
    /// </summary>
    public virtual string Message { get; init; } = default!;
    /// <summary>
    /// Gets the log exception
    /// </summary>
    public Exception? Exception { get; init; }

    /// <summary>
    /// Gets the log scopes
    /// </summary>
    public IList<object?>? Scopes { get; init; }
}
