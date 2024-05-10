namespace SSI.Extensions.Logging.TracesOnError;

public interface ITracesOnErrorFormatter {
    string Process(IList<LogEntry> logs);

    string ProcessMessagesOnly(IList<LogEntry> logs);

    void ScopeCallback(object? obj, IList<string?> state)
    {
        state.Add(obj?.ToString());
    }
}