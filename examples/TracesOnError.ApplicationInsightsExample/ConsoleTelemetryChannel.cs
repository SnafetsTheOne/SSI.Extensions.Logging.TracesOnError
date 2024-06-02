using Microsoft.ApplicationInsights.Channel;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Snafets.Extensions.Logging.TracesOnError.ApplicationInsightsExample
{
    public class ConsoleTelemetryChannel : ITelemetryChannel
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        };

        public void Send(ITelemetry item)
        {
            Console.WriteLine(JsonSerializer.Serialize(item, _jsonSerializerOptions));
        }

        public void Flush()
        {
            // nothing to do here
        }
        public void Dispose()
        {
            // nothing to do here
        }

        public bool? DeveloperMode { get; set; } = true;
        public string EndpointAddress { get; set; } = string.Empty;
    }
}
