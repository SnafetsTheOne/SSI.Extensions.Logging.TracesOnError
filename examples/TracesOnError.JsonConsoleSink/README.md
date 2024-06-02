# Snafets.Extensions.Logging.TracesOnError.JsonConsoleSink

Example to log to console in json format only when an error occurs.

## Setup

The custom log sink is registered like this:
``` csharp
builder.Logging.AddTracesOnError<JsonConsoleSink>();
```

## GET /test

will produce the following message in the console:
``` json
[
  {
    "Category": "Snafets.Extensions.Logging.TracesOnError.Console.Controllers.TestController",
    "LogLevel": "Trace",
    "Message": "TestController.Get"
  },
  {
    "Category": "Snafets.Extensions.Logging.TracesOnError.Console.Controllers.TestController",
    "LogLevel": "Error",
    "Message": "TestController.Get ExceptionHandler",
    "Exception": [
      "System.Exception: Test exception",
      "at Snafets.Extensions.Logging.TracesOnError.Console.Controllers.TestController.Get() in <path>\\examples\\TracesOnError.JsonConsoleSink\\Controllers\\TestController.cs:line 16"
    ]
  }
]
```
