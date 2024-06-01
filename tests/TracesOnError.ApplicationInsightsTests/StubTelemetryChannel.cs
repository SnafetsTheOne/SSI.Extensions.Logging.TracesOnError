using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Channel;

namespace Snafets.Extensions.Logging.TracesOnError.ApplicationInsightsTests
{
    internal class StubTelemetryChannel : ITelemetryChannel
    {
        public Action<ITelemetry> OnSend { get; set; } = (telemetry) => { };
        public Action OnFlush { get; set; } = () => { };
        public bool? DeveloperMode { get; set; } = true;
        public string EndpointAddress { get; set; } = "https://stub";

        public void Dispose()
        {
            // nothing to dispose
        }

        public void Send(ITelemetry item)
        {
            OnSend.Invoke(item);
        }

        public void Flush()
        {
            OnFlush.Invoke();
        }

    }
}
