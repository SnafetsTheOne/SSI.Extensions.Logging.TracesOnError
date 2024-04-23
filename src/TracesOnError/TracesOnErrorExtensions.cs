using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using System.Text;

namespace SSI.Extensions.Logging.TracesOnError
{
    public static class TracesOnErrorExtensions
    {
        private const string UninitializedMessage = "TracesOnError has not been initialized. Call AddTracesOnError first.";

        private static ITracesOnErrorStorageProvider StorageProvider => _storageProvider ?? 
            throw new InvalidOperationException(UninitializedMessage);
        private static ITracesOnErrorStorageProvider? _storageProvider;

        private static ITracesOnErrorLogSink LogSink => _logSink ?? 
            throw new InvalidOperationException(UninitializedMessage);
        private static ITracesOnErrorLogSink? _logSink;

        private static ITracesOnErrorFormatter Formatter => _formatter ??
            throw new InvalidOperationException(UninitializedMessage);
        private static ITracesOnErrorFormatter? _formatter;

        public static ILoggingBuilder AddTracesOnError(this ILoggingBuilder builder, ITracesOnErrorLogSink logSink)
        {
            builder.AddConfiguration();

            _storageProvider = new TracesOnErrorStorageProvider();
            _formatter = new TracesOnErrorFormatter();
            _logSink = logSink;

            builder.Services.AddOptions<TracesOnErrorOptions>().BindConfiguration(TracesOnErrorOptions.SectionName);
            builder.Services.AddSingleton<IConfigureOptions<LoggerFilterOptions>, TracesOnErrorConfigurationOptions>();

            builder.Services.AddSingleton<ILoggerProvider>(sp => 
                new TracesOnErrorLoggerProvider(logSink, StorageProvider, _formatter, sp.GetRequiredService<IOptionsMonitor<TracesOnErrorOptions>>()));
        
            return builder;
        }

        public static ILoggingBuilder AddTracesOnError(this ILoggingBuilder builder, Action<string> logSink, ITracesOnErrorFormatter? formatter = null)
        {
            builder.AddConfiguration();

            _storageProvider = new TracesOnErrorStorageProvider();
            _formatter = formatter ?? new TracesOnErrorFormatter();
            _logSink = new FormattedTracesOnErrorLogSink(Formatter, logSink);

            builder.Services.AddOptions<TracesOnErrorOptions>().BindConfiguration(TracesOnErrorOptions.SectionName);
            builder.Services.AddSingleton<IConfigureOptions<LoggerFilterOptions>, TracesOnErrorConfigurationOptions>();

            builder.Services.AddSingleton<ILoggerProvider>(sp =>
                new TracesOnErrorLoggerProvider(LogSink, StorageProvider, _formatter, sp.GetRequiredService<IOptionsMonitor<TracesOnErrorOptions>>()));

            return builder;
        }

        public static void LogErrorWithTraces<T>(this ILogger<T> logger, Exception exception, string message)
        {
            IList<LogEntry> logs;
            if(_storageProvider != null && _formatter != null & (logs = StorageProvider.GetLogs()).Any())
            {
                var traces = Formatter.ProcessMessagesOnly(logs) + "  " + message + Environment.NewLine;
                logger.LogError(exception, traces);
            }
            else
            {
                logger.LogError(exception, message);
            }
        }

        public static void LogErrorWithTraces<T>(this ILogger<T> logger, string message)
        {
            IList<LogEntry> logs;
            if (_storageProvider != null && _formatter != null & (logs = StorageProvider.GetLogs()).Any())
            {
                var traces = Formatter.ProcessMessagesOnly(logs) + "  " + message + Environment.NewLine;
                logger.LogError(traces);
            }
            else
            {
                logger.LogError(message);
            }
        }

        public static void LogCriticalWithTraces<T>(this ILogger<T> logger, Exception exception, string message)
        {
            IList<LogEntry> logs;
            if (_storageProvider != null && _formatter != null & (logs = StorageProvider.GetLogs()).Any())
            {
                var traces = Formatter.ProcessMessagesOnly(logs) + Environment.NewLine + "  " + message;
                logger.LogCritical(exception, traces);
            }
            else
            {
                logger.LogCritical(exception, message);
            }
        }

        public static void LogCriticalWithTraces<T>(this ILogger<T> logger, string message)
        {
            IList<LogEntry> logs;
            if (_storageProvider != null && _formatter != null & (logs = StorageProvider.GetLogs()).Any())
            {
                var traces = Formatter.ProcessMessagesOnly(logs) + Environment.NewLine + "  " + message;
                logger.LogCritical(traces);
            }
            else
            {
                logger.LogCritical(message);
            }
        }
    }
}