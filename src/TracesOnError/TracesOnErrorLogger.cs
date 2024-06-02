using Microsoft.Extensions.Logging;

namespace Snafets.Extensions.Logging.TracesOnError;

internal class TracesOnErrorLogger : ILogger
{
    public string Category { get; }
    private readonly ITracesOnErrorLogSink _logSink;
    private readonly ITracesOnErrorStorageProvider _storageProvider;
    public TracesOnErrorOptions Options { get; internal set; }
    public IExternalScopeProvider? ScopeProvider { get; internal set; }

    public TracesOnErrorLogger(string category, ITracesOnErrorLogSink logSink, ITracesOnErrorStorageProvider storageProvider
        , IExternalScopeProvider? scopeProvider, TracesOnErrorOptions options)
    {
        Category = category;
        _logSink = logSink;
        _storageProvider = storageProvider;
        ScopeProvider = scopeProvider;
        Options = options;
    }

    public void Log<TLogState>(LogLevel logLevel, EventId eventId, TLogState state, Exception? exception, Func<TLogState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;
        
        _storageProvider.AddLog(CreateLogEntry(logLevel, eventId, state, exception, formatter));

        if (logLevel < Options.ErrorThreshold)
            return;

        _logSink.WriteLog(_storageProvider.GetLogs());
    }

    private LogEntry CreateLogEntry<TState>(LogLevel logLevel, EventId eventId
        , TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        IList<object?>? scopes;
        if(Options.IncludeScopes)
        {
            scopes = new List<object?>();
            ScopeProvider?.ForEachScope((obj, aggregate) => aggregate.Add(obj), scopes);
        }
        else
        {
            scopes = null;
        }

        return new LogEntry<TState>
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