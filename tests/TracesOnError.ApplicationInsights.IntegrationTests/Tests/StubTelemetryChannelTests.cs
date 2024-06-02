using FluentAssertions;
using Microsoft.ApplicationInsights.DataContracts;
using Snafets.Extensions.Logging.TracesOnError.ApplicationInsights.IntegrationTests.Helpers;

namespace Snafets.Extensions.Logging.TracesOnError.ApplicationInsights.IntegrationTests.Tests;

public class StubTelemetryChannelTests
{
    [Fact]
    public async Task StubTelemetryChannel_GetNoErrorMessage_NoLogs()
    {
        var sink = new StubTelemetryChannel();
        await using var webApp = new CustomWebApplicationFactory(logging =>
        {
            logging.ClearProviders();
            logging.AddTracesOnErrorApplicationInsights(configure =>
            {
                configure.TelemetryChannel = sink;
            });
        });

        var response = await webApp.CreateClient().GetAsync("/Error/NoError");
        response.EnsureSuccessStatusCode();

        sink.SendTelemetryItems.Should().BeEmpty();
    }

    [Fact]
    public async Task StubTelemetryChannel_GetErrorMessage_TwoLogs()
    {
        var sink = new StubTelemetryChannel();
        await using var webApp = new CustomWebApplicationFactory(logging =>
        {
            logging.ClearProviders();
            logging.AddTracesOnErrorApplicationInsights(configure =>
            {
                configure.TelemetryChannel = sink;
            });
        });

        var response = await webApp.CreateClient().GetAsync("/Error/Message");
        response.EnsureSuccessStatusCode();

        sink.SendTelemetryItems.Should().HaveCount(1);
        var telemetry = sink.SendTelemetryItems[0];
        telemetry.Should().BeAssignableTo<TraceTelemetry>();
        var trace = (TraceTelemetry)telemetry;
        trace.Properties.Should().ContainKey("trace_0_Message");
        trace.Properties.Should().ContainKey("trace_1_Message");
    }

    [Fact]
    public async Task StubTelemetryChannel_GetErrorException_TwoLogs()
    {
        var sink = new StubTelemetryChannel();
        await using var webApp = new CustomWebApplicationFactory(logging =>
        {
            logging.ClearProviders();
            logging.AddTracesOnErrorApplicationInsights(configure =>
            {
                configure.TelemetryChannel = sink;
            });
        });

        var response = await webApp.CreateClient().GetAsync("/Error/Exception");
        response.EnsureSuccessStatusCode();

        sink.SendTelemetryItems.Should().HaveCount(1);
        var telemetry = sink.SendTelemetryItems[0];
        telemetry.Should().BeAssignableTo<ExceptionTelemetry>();
        var trace = (ExceptionTelemetry)telemetry;
        trace.Properties.Should().ContainKey("trace_0_Message");
        trace.Properties.Should().ContainKey("trace_1_Message");
    }
}
