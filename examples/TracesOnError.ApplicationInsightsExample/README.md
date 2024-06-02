# Snafets.Extensions.Logging.TracesOnError.ApplicationInsightsExample

Example to log all traces to application insights when an error occurs.

## Setup

Register AddTracesOnErrorApplicationInsights, for this example we register a ConsoleTelemetryChannel:
``` csharp
builder.Logging.AddTracesOnErrorApplicationInsights(configure =>
{
    configure.TelemetryChannel = new ConsoleTelemetryChannel();
});
```

## GET /test

produces the following message in the console:
``` json
{
  "Timestamp": "<the timestamp>",
  "Context": {
    "InstrumentationKey": "",
    "Component": {},
    "Device": {},
    "Cloud": {
      "RoleInstance": "<name>"
    },
    "Session": {},
    "User": {},
    "Operation": {
      "Id": "28bf55ca7bede22cf6e7477bf6295aaa",
      "ParentId": "9e280137e5bdeba4"
    },
    "Location": {},
    "Properties": {
      "trace_1_Exception": "System.Exception: Test exception\r\n   at Snafets.Extensions.Logging.TracesOnError.ApplicationInsights.Controllers.TestController.Get() in <path>\\examples\\TracesOnError.ApplicationInsightsExample\\Controllers\\TestConstroller.cs:line 16",
      "trace_0_CategoryName": "Snafets.Extensions.Logging.TracesOnError.ApplicationInsights.Controllers.TestController",
      "trace_1_LogLevel": "Error",
      "_MS.ProcessedByMetricExtractors": "(Name:\u0027Exceptions\u0027, Ver:\u00271.1\u0027)",
      "trace_0_LogLevel": "Trace",
      "DeveloperMode": "true",
      "trace_1_CategoryName": "Snafets.Extensions.Logging.TracesOnError.ApplicationInsights.Controllers.TestController",
      "trace_1_Message": "TestController.Get",
      "trace_0_Message": "TestController.Get"
    },
    "GlobalProperties": {}
  }
}
```
