namespace Snafets.Extensions.Logging.TracesOnError.JsonConsoleSink;

public class SerializableLogEntry
{
    public string Category { get; set; } = default!;
    public string LogLevel { get; set; } = default!;
    public EventId EventId { get; set; }
    public string Message { get; set; } = default!;
    public IList<string>? Exception { get; set; }
    public IList<object?>? Scopes { get; set; }

    public static SerializableLogEntry FromLogEntry(LogEntry logEntry)
    {
        return new SerializableLogEntry
        {
            Category = logEntry.Category,
            LogLevel = logEntry.LogLevel.ToString(),
            EventId = logEntry.EventId,
            Message = logEntry.Message,
            Exception = logEntry.Exception?.ToString().Split(Environment.NewLine).Select(x => x.Trim()).ToList(),
            Scopes = logEntry.Scopes
        };
    }
}

