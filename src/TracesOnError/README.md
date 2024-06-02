[![CI](https://github.com/SnafetsTheOne/Snafets.Extensions.Logging.TracesOnError/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/SnafetsTheOne/Snafets.Extensions.Logging.TracesOnError/actions/workflows/ci.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=snafetstheone_snafets-extensions-logging-tracesonerror&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=snafetstheone_snafets-extensions-logging-tracesonerror)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=snafetstheone_snafets-extensions-logging-tracesonerror&metric=coverage)](https://sonarcloud.io/summary/new_code?id=snafetstheone_snafets-extensions-logging-tracesonerror)

# Snafets.Extensions.Logging.TracesOnError

provides functionality to write all scoped logs into a single message when an error occurs.

# Usage

Register:
``` csharp
((ILoggingBuilder)builder).AddTracesOnError();
```

Register a custom log sink:
``` csharp
public class MyCustomLogSink : ITracesOnErrorLogSink
{ ... }

((ILoggingBuilder)builder).AddTracesOnError<MyCustomLogSink>();
```

Use logs in your code:
``` csharp
var tracesOnErrorStorageProvider = sp.GetRequiredService<ITracesOnErrorStorageProvider>();
var logs = tracesOnErrorStorageProvider.GetLogs();
```

# Links

- [GitHub](https://github.com/SnafetsTheOne/Snafets.Extensions.Logging.TracesOnError)
- [NuGet](https://www.nuget.org/packages/Snafets.Extensions.Logging.TracesOnError)
- [Examples](https://github.com/SnafetsTheOne/Snafets.Extensions.Logging.TracesOnError/tree/main/examples)
- [License](https://github.com/SnafetsTheOne/Snafets.Extensions.Logging.TracesOnError/blob/main/LICENSE)