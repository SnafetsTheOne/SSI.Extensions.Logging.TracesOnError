﻿using NSubstitute;

namespace SSI.Extensions.Logging.TracesOnError.IntegrationTests.Tests;

public class ActionSinkWithFormatterTests
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
        var sink = Substitute.For<Action<string>>();
        var formatter = Substitute.For<ITracesOnErrorFormatter>();

        var webApp = new CustomWebApplicationFactory(logging =>
        {
            logging.AddTracesOnError(sink, formatter);
        });

        var response = await webApp.CreateClient().GetAsync(url);
        response.EnsureSuccessStatusCode();

        sink.Received(1).Invoke(Arg.Any<string>());
        formatter.Received(1).Process(Arg.Any<IList<LogEntry>>());
    }
}
