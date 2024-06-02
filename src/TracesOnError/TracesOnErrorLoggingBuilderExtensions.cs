using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace Snafets.Extensions.Logging.TracesOnError;

/// <summary>
/// Extension methods for adding TracesOnError to the logging pipeline.
/// </summary>
public static class TracesOnErrorLoggingBuilderExtensions
{
    /// <summary>
    /// Adds TracesOnError logger named 'TracesOnError' to the factory.
    /// </summary>
    /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
    /// <param name="configureTracesOnErrorOptions">Action to configure the logger options.</param>
    public static ILoggingBuilder AddTracesOnError(this ILoggingBuilder builder
        , Action<TracesOnErrorOptions>? configureTracesOnErrorOptions = null) 
        => builder.AddTracesOnError<NullTracesOnErrorLogSink, DefaultTracesOnErrorStorageProvider>(configureTracesOnErrorOptions);

    /// <summary>
    /// Adds TracesOnError logger named 'TracesOnError' to the factory.
    /// </summary>
    /// <typeparam name="TLogSink">the type to the <see cref="ITracesOnErrorLogSink"/> to use.</typeparam>
    /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
    /// <param name="configureTracesOnErrorOptions">Action to configure the logger options.</param>
    public static ILoggingBuilder AddTracesOnError<TLogSink>(this ILoggingBuilder builder
        , Action<TracesOnErrorOptions>? configureTracesOnErrorOptions = null)
        where TLogSink : class, ITracesOnErrorLogSink 
        => builder.AddTracesOnError<TLogSink, DefaultTracesOnErrorStorageProvider>(configureTracesOnErrorOptions);

    /// <summary>
    /// Adds TracesOnError logger named 'TracesOnError' to the factory.
    /// </summary>
    /// <typeparam name="TLogSink">the type to the <see cref="ITracesOnErrorLogSink"/> to use.</typeparam>
    /// <typeparam name="TStorageProvider">the type of the <see cref="ITracesOnErrorStorageProvider"/> to use.</typeparam>
    /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
    /// <param name="configureTracesOnErrorOptions">Action to configure the logger options.</param>
    public static ILoggingBuilder AddTracesOnError<TLogSink, TStorageProvider>(this ILoggingBuilder builder
        , Action<TracesOnErrorOptions>? configureTracesOnErrorOptions = null)
        where TLogSink : class, ITracesOnErrorLogSink
        where TStorageProvider : class, ITracesOnErrorStorageProvider
    {
        builder.AddConfiguration();

        builder.Services.Configure(configureTracesOnErrorOptions ?? (configure => { }));
        builder.Services.AddSingleton<ITracesOnErrorLogSink, TLogSink>();
        builder.Services.AddSingleton<ITracesOnErrorStorageProvider, TStorageProvider>();

        builder.Services.AddSingleton<IConfigureOptions<LoggerFilterOptions>, TracesOnErrorConfigurationOptions>();

        builder.Services.AddSingleton<ILoggerProvider, TracesOnErrorLoggerProvider>();

        return builder;
    }

    /// <summary>
    /// Adds TracesOnError logger named 'TracesOnError' to the factory.
    /// </summary>
    /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
    /// <param name="logSink">the <see cref="ITracesOnErrorLogSink"/> to use.</param>
    /// <param name="storageProvider">the <see cref="ITracesOnErrorStorageProvider"/> to use.</param>
    /// <param name="configureTracesOnErrorOptions">Action to configure the logger options.</param>
    public static ILoggingBuilder AddTracesOnError(this ILoggingBuilder builder
        , ITracesOnErrorLogSink logSink
        , ITracesOnErrorStorageProvider? storageProvider = null
        , Action<TracesOnErrorOptions>? configureTracesOnErrorOptions = null)
    {
        builder.AddConfiguration();

        builder.Services.Configure(configureTracesOnErrorOptions ?? (configure => { }));
        builder.Services.AddSingleton(logSink);
        builder.Services.AddSingleton(storageProvider ?? new DefaultTracesOnErrorStorageProvider());

        builder.Services.AddSingleton<IConfigureOptions<LoggerFilterOptions>, TracesOnErrorConfigurationOptions>();

        builder.Services.AddSingleton<ILoggerProvider, TracesOnErrorLoggerProvider>();

        return builder;
    }
}