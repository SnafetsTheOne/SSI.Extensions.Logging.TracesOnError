using Microsoft.Extensions.Logging;

namespace Snafets.Extensions.Logging.TracesOnError;

internal record LogEntry<TLogState> : LogEntry
{
    public TLogState State { get; init; } = default!;
    public Func<TLogState, Exception?, string> Formatter { get; init; } = default!;
    public override string Message => Formatter(State, Exception);
}

public record LogEntry
{
    public string Category { get; init; } = default!;
    public LogLevel LogLevel { get; init; }
    public EventId EventId { get; init; }
    public IList<string?> Scopes { get; init; } = default!;
    public virtual string Message { get; init; } = default!;
    public Exception? Exception { get; init; }
}