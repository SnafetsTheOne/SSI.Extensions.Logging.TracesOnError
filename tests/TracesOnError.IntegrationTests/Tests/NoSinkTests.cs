namespace SSI.Extensions.Logging.TracesOnError.IntegrationTests.Tests;

[Collection("web")]
public class NoSinkTests
{
    [Theory]
    [InlineData("/Sink/Message")]
    [InlineData("/Sink/Exception")]
    [InlineData("/Enriched/Error/Message")]
    [InlineData("/Enriched/Error/Exception")]
    [InlineData("/Enriched/Critical/Message")]
    [InlineData("/Enriched/Critical/Exception")]
    public async Task WithoutLogSink(string url)
    {
        await using var webApp = new CustomWebApplicationFactory(logging =>
        {
            logging.AddTracesOnErrorWithoutLogSink();
        });

        var response = await webApp.CreateClient().GetAsync(url);
        response.EnsureSuccessStatusCode();
    }
}

