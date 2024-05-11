using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Snafets.Extensions.Logging.TracesOnError;

internal class TracesOnErrorConfigurationOptions : IConfigureOptions<LoggerFilterOptions>
{
    private readonly IOptionsMonitor<TracesOnErrorOptions> _options;

    public TracesOnErrorConfigurationOptions(IOptionsMonitor<TracesOnErrorOptions> options)
    {
        _options = options;
    }

    public void Configure(LoggerFilterOptions options)
    {
        // If the default log level is not set, add a rule to log all traces
        if (!_options.CurrentValue.LogLevel.ContainsKey("Default"))
        {
            options.Rules.Add(new LoggerFilterRule("TracesOnError", null, LogLevel.Trace, null));
        }

        // as the above rule overrides the default, we need to add a rule for Microsoft.AspNetCore
        if (!_options.CurrentValue.LogLevel.ContainsKey("Microsoft.AspNetCore"))
        {
            options.Rules.Add(new LoggerFilterRule("TracesOnError", "Microsoft.AspNetCore", LogLevel.Warning, null));
        }
    }
}