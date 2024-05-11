# Snafets.Extensions.Logging.TracesOnError

provides functionality to enrich log messages with all previous traces

## Usage

### Register Services

``` csharp
WebApplicationBuilder builder = ...;
builder.Logging.AddTracesOnError(logSink: Console.WriteLine);
```
``` csharp
WebApplicationBuilder builder = ...;
builder.Logging.AddTracesOnErrorWithoutLogSink();
```

### Logging

``` csharp
ILogger logger = ...;
logger.LogError(new Exception("error message"), "error message");
```

``` csharp
ILogger logger = ...;
logger.LogErrorWithTraces(exception, "message");
```

## Examples

[examples](https://github.com/SnafetsTheOne/Snafets.Extensions.Logging.TracesOnError/tree/main/examples)