using System.Text.Json;
using System.Text.Json.Serialization;

namespace Snafets.Extensions.Logging.TracesOnError.JsonConsoleSink
{
    public class JsonConsoleSink : ITracesOnErrorLogSink
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new ()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        };

        public void WriteLog(IReadOnlyList<LogEntry> logs)
        {
            System.Console.WriteLine(JsonSerializer.Serialize(logs.Select(SerializableLogEntry.FromLogEntry), _jsonSerializerOptions));
        }
    }
}
