using Microsoft.AspNetCore.Mvc.Testing;

namespace Snafets.Extensions.Logging.TracesOnError.IntegrationTests;

internal class CustomWebApplicationFactory(Action<ILoggingBuilder> configureLogging) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(configureLogging);
    }
}