using Microsoft.AspNetCore.Mvc.Testing;

namespace SSI.Extensions.Logging.TracesOnError.IntegrationTests;

internal class CustomWebApplicationFactory(Action<ILoggingBuilder> configureLogging) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(configureLogging);
    }
}