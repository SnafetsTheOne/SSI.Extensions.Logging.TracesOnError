using Microsoft.Extensions.Logging;

namespace SSI.Extensions.Logging.TracesOnError;

internal class TracesOnErrorLogger : ILogger
{
    public string Category { get; }
    private readonly ITracesOnErrorLogSink _logSink;
    private readonly ITracesOnErrorStorageProvider _storageProvider;
    private readonly ITracesOnErrorFormatter _formatter;
    public TracesOnErrorOptions Options { get; internal set; }
    public IExternalScopeProvider? ScopeProvider { get; internal set; }

    public TracesOnErrorLogger(string category, ITracesOnErrorLogSink logSink, ITracesOnErrorStorageProvider storageProvider
        , ITracesOnErrorFormatter formatter, IExternalScopeProvider? scopeProvider, TracesOnErrorOptions options)
    {
        Category = category;
        _logSink = logSink;
        _storageProvider = storageProvider;
        _formatter = formatter;
        ScopeProvider = scopeProvider;
        Options = options;
    }

    public void Log<TLogState>(LogLevel logLevel, EventId eventId, TLogState state, Exception? exception, Func<TLogState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;
        
        var logs = _storageProvider.GetLogs();

        logs.Add(CreateLogEntry(logLevel, eventId, state, exception, formatter));

        if (logLevel < Options.ErrorThreshold)
            return;

        _logSink.WriteLog(logs);
    }

    private LogEntry CreateLogEntry<TLogState>(LogLevel logLevel, EventId eventId
        , TLogState state, Exception? exception, Func<TLogState, Exception?, string> formatter)
    {
        IList<string?> scopes;
        if(Options.IncludeScopes)
        {
            scopes = new List<string?>();
            ScopeProvider?.ForEachScope(_formatter.ScopeCallback, scopes);
        }
        else
        {
            scopes = Array.Empty<string?>();
        }

        return new LogEntry<TLogState>
        {
            Category = Category,
            LogLevel = logLevel,
            EventId = eventId,
            State = state,
            Scopes = scopes,
            Exception = exception,
            Formatter = formatter
        };
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return Options.IncludeScopes ? ScopeProvider?.Push(state) ?? NullScope.Instance : NullScope.Instance;
    }
}