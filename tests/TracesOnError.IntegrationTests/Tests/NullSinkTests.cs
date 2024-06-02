using System.Net;
using FluentAssertions;

namespace Snafets.Extensions.Logging.TracesOnError.IntegrationTests.Tests;

[Collection("web")]
public class NullSinkTests
{
    [Theory]
    [InlineData("/Error/NoError")]
    [InlineData("/Error/Message")]
    [InlineData("/Error/Exception")]
    public async Task TracesOnError_WithoutLogSink_Success(string url)
    {
        await using var webApp = new CustomWebApplicationFactory(logging =>
        {
            logging.ClearProviders();
            logging.AddTracesOnError();
        });

        var response = await webApp.CreateClient().GetAsync(url);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}

