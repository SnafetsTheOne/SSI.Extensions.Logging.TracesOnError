[![CI](https://github.com/SnafetsTheOne/Snafets.Extensions.Logging.TracesOnError/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/SnafetsTheOne/Snafets.Extensions.Logging.TracesOnError/actions/workflows/ci.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=snafetstheone_snafets-extensions-logging-tracesonerror&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=snafetstheone_snafets-extensions-logging-tracesonerror)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=snafetstheone_snafets-extensions-logging-tracesonerror&metric=coverage)](https://sonarcloud.io/summary/new_code?id=snafetstheone_snafets-extensions-logging-tracesonerror)

# Snafets.Extensions.Logging.TracesOnError

provides functionality to write all scoped logs into a single message when an error occurs.

## Motivation

When an exception is thrown it is often hard to understand the context of the error. 
This library provides a way to enrich the log message with all previous traces.
This can be very helpful to understand the context of the error.
Enabling `LogLevel == Trace` is often not feasable as in modern cloud environements the cost of logging can often exeed the cost of the application itself.
Also the distribute logs have to be collected and aggregated by queries to the log storage.
By only logging debug information when an error occurs the cost of logging can be reduced, while maintaining the ability to understand the context of the error.

## Usage

### Setup

During Setup register `TracesOnError`, also some parameters are possible to add a logsink or to setup some options.
``` csharp
((ILoggingBuilder)builder).AddTracesOnError();
```

### Access to all current logs

By using this libarary you have access to all current logs by injecting the `ITracesOnErrorStorageProvider` to you class and calling `GetLogs()`.

### Custom Log Sink

configure your own log sink that gets called when a log message is written that exceeds the threshold.
You might want to write the logs to a file, a database or a cloud service.

``` cshrp
public class MyCustomLogSink : ITracesOnErrorLogSink
{ ... }
```

register the custom log sink like this:
``` cshrp
builder.Logging.AddTracesOnError<MyCustomLogSink>();
```

see [Example: JsonConsoleSink](https://github.com/SnafetsTheOne/Snafets.Extensions.Logging.TracesOnError/tree/main/examples/TracesOnError.JsonConsoleSink)

### Application Insights

we provide a Application Insights integration via the package `Snafets.Extensions.Logging.TracesOnError.ApplicationInsights`.

see [Package: TracesOnError.ApplicationInsights](https://github.com/SnafetsTheOne/Snafets.Extensions.Logging.TracesOnError/tree/main/src/TracesOnError.ApplicationInsights)

## Examples

Look for some example implementations in our [examples](https://github.com/SnafetsTheOne/Snafets.Extensions.Logging.TracesOnError/tree/main/examples) folder.

## How can I contribute?

You are welcome to contribute!

## License

Snafets.Extensions.Logging.TracesOnError is licensed under the [MIT](LICENSE.TXT) license.

## Code Quality

This Project uses SonarCloud for code quality checks. 
You can find the latest analysis at the [SonarCloud Dashboard](https://sonarcloud.io/project/overview?id=snafetstheone_snafets-extensions-logging-tracesonerror).

## NuGet.org

Find our packages on [Nuget.org](https://www.nuget.org/profiles/SnafetsTheOne):
- [Snafets.Extensions.Logging.TracesOnError](https://www.nuget.org/packages/Snafets.Extensions.Logging.TracesOnError/)
- [Snafets.Extensions.Logging.TracesOnError.ApplicationInsights](https://www.nuget.org/packages/Snafets.Extensions.Logging.TracesOnError.ApplicationInsights/)
