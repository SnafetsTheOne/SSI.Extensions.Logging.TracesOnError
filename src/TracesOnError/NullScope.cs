namespace Snafets.Extensions.Logging.TracesOnError;

internal sealed class NullScope : IDisposable
{
    public static NullScope Instance { get; } = new();
    
    public void Dispose()
    {
    }
}
