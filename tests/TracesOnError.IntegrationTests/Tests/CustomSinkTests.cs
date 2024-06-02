using FluentAssertions;
using Snafets.Extensions.Logging.TracesOnError.IntegrationTests.Helpers;

namespace Snafets.Extensions.Logging.TracesOnError.IntegrationTests.Tests;

[Collection("web")]
public class CustomSinkTests
{
    [Fact]
    public async Task GetNoErrorMessage_CustomSink_NoLogs()
    {
        var sink = new CustomSink();
        await using var webApp = new CustomWebApplicationFactory(logging =>
        {
            logging.ClearProviders();
            logging.AddTracesOnError(sink);
        });

        var response = await webApp.CreateClient().GetAsync("/Error/NoError");
        response.EnsureSuccessStatusCode();

        sink.Logs.Should().BeEmpty();
    }

    [Fact]
    public async Task GetErrorMessage_CustomSink_ExpectedResults()
    {
        var sink = new CustomSink();
        await using var webApp = new CustomWebApplicationFactory(logging =>
        {
            logging.ClearProviders();
            logging.AddTracesOnError(sink);
        });

        var response = await webApp.CreateClient().GetAsync("/Error/Message");
        response.EnsureSuccessStatusCode();

        sink.Logs.Should().HaveCount(1);
        var logs = sink.Logs[0];
        logs.Should().NotBeNull();
        logs.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetErrorExceptionMessage_CustomSink_ExpectedResults()
    {
        var sink = new CustomSink();
        await using var webApp = new CustomWebApplicationFactory(logging =>
        {
            logging.ClearProviders();
            logging.AddTracesOnError(sink);
        });

        var response = await webApp.CreateClient().GetAsync("/Error/Exception");
        response.EnsureSuccessStatusCode();

        sink.Logs.Should().HaveCount(1);
        var logs = sink.Logs[0];
        logs.Should().NotBeNull();
        logs.Should().HaveCount(2);
    }
}
