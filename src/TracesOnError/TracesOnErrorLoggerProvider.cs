using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Snafets.Extensions.Logging.TracesOnError;

[ProviderAlias("TracesOnError")]
internal class TracesOnErrorLoggerProvider : ILoggerProvider, ISupportExternalScope
{
    private readonly IOptionsMonitor<TracesOnErrorOptions> _options;
    private readonly ConcurrentDictionary<string, TracesOnErrorLogger> _loggers;
    private readonly ITracesOnErrorLogSink _logSink;
    private readonly ITracesOnErrorStorageProvider _storageProvider;
    private readonly IDisposable? _optionsReloadToken;
    private IExternalScopeProvider? _scopeProvider;
    
    public TracesOnErrorLoggerProvider(ITracesOnErrorLogSink logSink, ITracesOnErrorStorageProvider storageProvider, IOptionsMonitor<TracesOnErrorOptions> option)
    {
        _logSink = logSink;
        _storageProvider = storageProvider;
        _options = option;
        _scopeProvider = null;
        _loggers = new ConcurrentDictionary<string, TracesOnErrorLogger>();

        ReloadLoggerOptions(_options.CurrentValue);
        _optionsReloadToken = _options.OnChange(ReloadLoggerOptions);
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => 
            new TracesOnErrorLogger(name, _logSink, _storageProvider, _scopeProvider, _options.CurrentValue));
    }

    private void ReloadLoggerOptions(TracesOnErrorOptions options) 
    {
        foreach (var logger in _loggers)
        {
            logger.Value.Options = options;
        }
    }

    public void SetScopeProvider(IExternalScopeProvider scopeProvider)
    {
        _scopeProvider = scopeProvider;

        foreach (var logger in _loggers)
        {
            logger.Value.ScopeProvider = _scopeProvider;
        }
    }
    
    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            _optionsReloadToken?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}