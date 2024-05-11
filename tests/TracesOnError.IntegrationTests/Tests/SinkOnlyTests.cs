using NSubstitute;

namespace SSI.Extensions.Logging.TracesOnError.IntegrationTests.Tests;

[Collection("web")]
public class SinkOnlyTests
{
    [Theory]
    [InlineData("/Sink/Message")]
    [InlineData("/Sink/Exception")]
    [InlineData("/Enriched/Error/Message")]
    [InlineData("/Enriched/Error/Exception")]
    [InlineData("/Enriched/Critical/Message")]
    [InlineData("/Enriched/Critical/Exception")]
    public async Task SinkOnly(string url)
    {
        var sink = Substitute.For<ITracesOnErrorLogSink>();

        await using var webApp = new CustomWebApplicationFactory(logging =>
        {
            logging.ClearProviders();
            logging.AddTracesOnError(sink);
        });

        var response = await webApp.CreateClient().GetAsync(url);
        response.EnsureSuccessStatusCode();

        sink.Received(1).WriteLog(Arg.Any<IList<LogEntry>>());
    }
}

