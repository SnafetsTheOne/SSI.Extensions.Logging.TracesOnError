using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Snafets.Extensions.Logging.TracesOnError;

internal class TracesOnErrorConfigurationOptions : IConfigureOptions<LoggerFilterOptions>
{
    private readonly IOptions<TracesOnErrorOptions> _options;

    public TracesOnErrorConfigurationOptions(IOptions<TracesOnErrorOptions> options)
    {
        _options = options;
    }

    public void Configure(LoggerFilterOptions options)
    {
        options.Rules.Add(new LoggerFilterRule("TracesOnError", null, _options.Value.MinimumLevelToInclude, null));
        options.Rules.Add(new LoggerFilterRule("TracesOnError", "Microsoft.AspNetCore", _options.Value.AspNetCoreLogLevel, null));
    }
}