namespace SSI.Extensions.Logging.TracesOnError.IntegrationTests.Tests;

public class NoSinkTests
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
        var webApp = new CustomWebApplicationFactory(logging =>
        {
            logging.AddTracesOnErrorWithoutLogSink();
        });

        var response = await webApp.CreateClient().GetAsync(url);
        response.EnsureSuccessStatusCode();
    }
}

