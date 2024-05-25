[![CI](https://github.com/SnafetsTheOne/Snafets.Extensions.Logging.TracesOnError/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/SnafetsTheOne/Snafets.Extensions.Logging.TracesOnError/actions/workflows/ci.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=snafetstheone_snafets-extensions-logging-tracesonerror&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=snafetstheone_snafets-extensions-logging-tracesonerror)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=snafetstheone_snafets-extensions-logging-tracesonerror&metric=coverage)](https://sonarcloud.io/summary/new_code?id=snafetstheone_snafets-extensions-logging-tracesonerror)

# Snafets.Extensions.Logging.TracesOnError

provides functionality to enrich log messages with all previous traces

## Usage

During Setup Register `TracesOnError`
``` csharp
IWebApplicationBuilder builder = ...;
builder.Logging.AddTracesOnError(logSink: Console.WriteLine);
```
or
``` csharp
IWebApplicationBuilder builder = ...;
builder.Logging.AddTracesOnErrorWithoutLogSink();
```

Log Messages normally 
``` csharp
((ILogger)logger).LogError(exception, "error message");
```
Or use the new extension methods
``` csharp
((ILogger)logger).LogErrorWithTraces(exception, "error message");
```

## Examples

For example implementations see [examples](https://github.com/SnafetsTheOne/Snafets.Extensions.Logging.TracesOnError/tree/main/examples)

## How can I contribute?

You are welcome to contribute!

## License

Snafets.Extensions.Logging.TracesOnError is licensed under the [MIT](LICENSE.TXT) license.

## Code Quality

This Project uses SonarCloud for code quality checks. You can find the latest analysis at the [SonarCloud Dashboard](https://sonarcloud.io/project/configuration?id=snafetstheone_snafets-extensions-logging-tracesonerror).