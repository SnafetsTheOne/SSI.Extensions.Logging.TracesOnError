using Microsoft.Extensions.Options;

namespace Snafets.Extensions.Logging.TracesOnError.Tests.Helper;

internal class TestMonitorOptions<TOptions> : IOptionsMonitor<TOptions>, IDisposable 
{
    private readonly List<Action<TOptions, string?>> _listeners = new();

    public TestMonitorOptions(TOptions options)
    {
        _options = options;
    }

    public TOptions Get(string? name)
    {
        return CurrentValue;
    }

    public IDisposable? OnChange(Action<TOptions, string?> listener)
    {
        _listeners.Add(listener);
        return this;
    }

    private TOptions _options;

    public TOptions CurrentValue
    {
        get => _options;
        set
        {
            _options = value;
            foreach (var listener in _listeners)
            {
                listener(value, null);
            }
        }
    }

    public void Dispose()
    {

    }
}

