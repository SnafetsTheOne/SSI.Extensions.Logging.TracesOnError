[![CI](https://github.com/SnafetsTheOne/Snafets.Extensions.Logging.TracesOnError/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/SnafetsTheOne/Snafets.Extensions.Logging.TracesOnError/actions/workflows/ci.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=snafetstheone_snafets-extensions-logging-tracesonerror&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=snafetstheone_snafets-extensions-logging-tracesonerror)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=snafetstheone_snafets-extensions-logging-tracesonerror&metric=coverage)](https://sonarcloud.io/summary/new_code?id=snafetstheone_snafets-extensions-logging-tracesonerror)

# Snafets.Extensions.Logging.TracesOnError

provides functionality to enrich log messages with all previous traces

## Motivation

When an exception is thrown it is often hard to understand the context of the error. This library provides a way to enrich the log message with all previous traces. This can be very helpful to understand the context of the error. Enabling `LogLevel == Trace` is often not feasable as in modern cloud environements the cost of logging can often exeed the cost of the application itself. By only logging debug information when an error occurs the cost of logging can be reduced.

## Usage

During Setup register `TracesOnError`, also some parameters are possible to add a logsink or to setup some options.
``` csharp
((ILoggingBuilder)builder).AddTracesOnError();
```

Log Messages normally, which has no effects on other loggers. But when specifying a `ITracesOnErrorLogSink` then the error messages are writen to that sink.
``` csharp
((ILogger)logger).LogError(exception, "error message");
```
To encance the log messages of other loggers use our new extension methods for `ILogger`:
``` csharp
((ILogger)logger).LogErrorWithTraces(exception, "error message");
```

### Application Insights

To use Application Insights as a log sink, you can use the `Snafets.Extensions.Logging.TracesOnError.ApplicationInsights` package. 

``` csharp
TelemetryClient telemetryClient = ...; // initialize the client as you would normally do
((ILoggingBuilder)builder).AddTracesOnErrorApplicationInsights(telemetryClient);
```

## Examples

Look for some example implementations in our [examples](https://github.com/SnafetsTheOne/Snafets.Extensions.Logging.TracesOnError/tree/main/examples) folder.

## How can I contribute?

You are welcome to contribute!

## License

Snafets.Extensions.Logging.TracesOnError is licensed under the [MIT](LICENSE.TXT) license.

## Code Quality

This Project uses SonarCloud for code quality checks. You can find the latest analysis at the [SonarCloud Dashboard](https://sonarcloud.io/project/overview?id=snafetstheone_snafets-extensions-logging-tracesonerror).

## Nuget.org

The package is available on [Nuget.org](https://www.nuget.org/packages/Snafets.Extensions.Logging.TracesOnError/)
