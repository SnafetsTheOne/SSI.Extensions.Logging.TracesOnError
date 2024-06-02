using Microsoft.ApplicationInsights.Channel;

namespace Snafets.Extensions.Logging.TracesOnError.ApplicationInsights.IntegrationTests.Helpers
{
    internal class StubTelemetryChannel : ITelemetryChannel
    {
        public List<ITelemetry> SendTelemetryItems { get; } = new List<ITelemetry>();
        public bool? DeveloperMode { get; set; } = true;
        public string EndpointAddress { get; set; } = "https://stub";

        public void Send(ITelemetry item)
        {
            SendTelemetryItems.Add(item);
        }

        public void Flush()
        {
            // nothing to do
        }

        public void Dispose()
        {
            // nothing to dispose
        }
    }
}
