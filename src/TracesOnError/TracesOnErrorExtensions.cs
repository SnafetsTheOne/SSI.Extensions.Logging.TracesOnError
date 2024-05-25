using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using System.Text;

namespace Snafets.Extensions.Logging.TracesOnError;

/// <summary>
/// Extension methods for adding TracesOnError to the logging pipeline.
/// </summary>
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

    /// <summary>
    /// Adds TracesOnError Logger to the logging pipeline.
    /// </summary>
    /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
    /// <param name="logSink">the log sink to use; <see cref="NullTracesOnErrorLogSink"/> if null.</param>
    /// <param name="formatter">the log message formatter to use. <see cref="TracesOnErrorFormatter"/> if null.</param>
    public static ILoggingBuilder AddTracesOnError(this ILoggingBuilder builder, ITracesOnErrorLogSink? logSink = null, ITracesOnErrorFormatter? formatter = null)
    {
        builder.AddConfiguration();

        _storageProvider = new TracesOnErrorStorageProvider();
        _formatter = formatter ?? new TracesOnErrorFormatter();
        _logSink = logSink ?? NullTracesOnErrorLogSink.Instance;

        builder.Services.AddOptions<TracesOnErrorOptions>().BindConfiguration(TracesOnErrorOptions.SectionName);
        builder.Services.AddSingleton<IConfigureOptions<LoggerFilterOptions>, TracesOnErrorConfigurationOptions>();

        builder.Services.AddSingleton<ILoggerProvider>(sp => 
            new TracesOnErrorLoggerProvider(logSink, StorageProvider, _formatter, sp.GetRequiredService<IOptionsMonitor<TracesOnErrorOptions>>()));
        
        return builder;
    }

    /// <summary>
    /// Adds TracesOnError Logger to the logging pipeline.
    /// </summary>
    /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
    /// <param name="logSink">the log sink to use</param>
    /// <param name="formatter">the log message formatter to use. <see cref="TracesOnErrorFormatter"/> if null.</param>
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

    /// <summary>
    /// Adds AddTracesOnError to the logging pipeline.
    /// </summary>
    /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
    /// <param name="formatter"></param>
    [Obsolete("Use AddTracesOnError instead")]
    public static ILoggingBuilder AddTracesOnErrorWithoutLogSink(this ILoggingBuilder builder, ITracesOnErrorFormatter? formatter = null)
    {
        return AddTracesOnError(builder, formatter: formatter);
    }

    /// <summary>
    /// Writes an error log message that includes all previous log entries.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message.</param>
    public static void LogErrorWithTraces<T>(this ILogger<T> logger, Exception exception, string message)
    {
        IList<LogEntry> logs;
        if(_storageProvider != null && _formatter != null && (logs = StorageProvider.GetLogs()).Any())
        {
            var traces = Formatter.ProcessMessagesOnly(logs) + "  " + message + Environment.NewLine;
            logger.LogError(exception, traces);
        }
        else
        {
            logger.LogError(exception, message);
        }
    }

    /// <summary>
    /// Writes an error log message that includes all previous log entries.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="message">Format string of the log message.</param>
    public static void LogErrorWithTraces<T>(this ILogger<T> logger, string message)
    {
        IList<LogEntry> logs;
        if (_storageProvider != null && _formatter != null && (logs = StorageProvider.GetLogs()).Any())
        {
            var traces = Formatter.ProcessMessagesOnly(logs) + "  " + message + Environment.NewLine;
            logger.LogError(traces);
        }
        else
        {
            logger.LogError(message);
        }
    }

    /// <summary>
    /// Writes a critical log message that includes all previous log entries.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message.</param>
    public static void LogCriticalWithTraces<T>(this ILogger<T> logger, Exception exception, string message)
    {
        IList<LogEntry> logs;
        if (_storageProvider != null && _formatter != null && (logs = StorageProvider.GetLogs()).Any())
        {
            var traces = Formatter.ProcessMessagesOnly(logs) + Environment.NewLine + "  " + message;
            logger.LogCritical(exception, traces);
        }
        else
        {
            logger.LogCritical(exception, message);
        }
    }

    /// <summary>
    /// Writes a critical log message that includes all previous log entries.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="message">Format string of the log message.</param>
    public static void LogCriticalWithTraces<T>(this ILogger<T> logger, string message)
    {
        IList<LogEntry> logs;
        if (_storageProvider != null && _formatter != null && (logs = StorageProvider.GetLogs()).Any())
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