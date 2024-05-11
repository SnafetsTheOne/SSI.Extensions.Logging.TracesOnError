using NSubstitute;

namespace Snafets.Extensions.Logging.TracesOnError.IntegrationTests.Tests;

[Collection("web")]
public class ActionSinkWithFormatterTests
{
    [Theory]
    [InlineData("/Sink/Message")]
    [InlineData("/Sink/Exception")]
    [InlineData("/Enriched/Error/Message")]
    [InlineData("/Enriched/Error/Exception")]
    [InlineData("/Enriched/Critical/Message")]
    [InlineData("/Enriched/Critical/Exception")]
    public async Task SinkAndFormatter(string url)
    {
        var sink = Substitute.For<Action<string>>();
        var formatter = Substitute.For<ITracesOnErrorFormatter>();

        await using var webApp = new CustomWebApplicationFactory(logging =>
        {
            logging.ClearProviders();
            logging.AddTracesOnError(sink, formatter);
        });

        var response = await webApp.CreateClient().GetAsync(url);
        response.EnsureSuccessStatusCode();

        sink.Received(1).Invoke(Arg.Any<string>());
        formatter.Received(1).Process(Arg.Any<IList<LogEntry>>());
    }
}

