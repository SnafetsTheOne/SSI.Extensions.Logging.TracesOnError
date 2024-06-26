﻿using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Snafets.Extensions.Logging.TracesOnError.Tests.Helper;

namespace Snafets.Extensions.Logging.TracesOnError.Tests.Tests;

public class TracesOnErrorLoggerProviderTests
{
    [Fact]
    public void CreateLogger()
    {
        var logSink = Substitute.For<ITracesOnErrorLogSink>();
        var storageProvider = Substitute.For<ITracesOnErrorStorageProvider>();
        var options = new TracesOnErrorOptions();
        var optionsMonitor = Substitute.For<IOptionsMonitor<TracesOnErrorOptions>>();
        optionsMonitor.CurrentValue.Returns(options);
        var sut = new TracesOnErrorLoggerProvider(logSink, storageProvider, optionsMonitor);

        var logger = sut.CreateLogger("Test");
        logger.Should().NotBeNull();

        var nextLogger = sut.CreateLogger("Test");
        nextLogger.Should().BeSameAs(logger);
    }

    [Fact]
    public void ReloadLoggerOptions()
    {
        var logSink = Substitute.For<ITracesOnErrorLogSink>();
        var storageProvider = Substitute.For<ITracesOnErrorStorageProvider>();
        var options = new TracesOnErrorOptions();
        var optionsMonitor = new TestMonitorOptions<TracesOnErrorOptions>(options);
        optionsMonitor.CurrentValue.Returns(options);
        var sut = new TracesOnErrorLoggerProvider(logSink, storageProvider, optionsMonitor);

        var logger = sut.CreateLogger("Test");
        logger.Should().BeOfType<TracesOnErrorLogger>();
        var tracesOnErrorLogger = (TracesOnErrorLogger)logger;
        tracesOnErrorLogger.Options.ErrorThreshold.Should().Be(options.ErrorThreshold);

        optionsMonitor.CurrentValue = new TracesOnErrorOptions()
        {
            ErrorThreshold = LogLevel.Error
        };

        tracesOnErrorLogger.Options.ErrorThreshold.Should().Be(LogLevel.Error);
    }

    [Fact]
    public void SetScopeProvider_BeforeLoggerCreation_ScopeProviderIsSet()
    {
        var logSink = Substitute.For<ITracesOnErrorLogSink>();
        var storageProvider = Substitute.For<ITracesOnErrorStorageProvider>();
        var options = new TracesOnErrorOptions();
        var optionsMonitor = Substitute.For<IOptionsMonitor<TracesOnErrorOptions>>();
        optionsMonitor.CurrentValue.Returns(options);
        var sut = new TracesOnErrorLoggerProvider(logSink, storageProvider, optionsMonitor);

        var scopeProvider = Substitute.For<IExternalScopeProvider>();

        sut.SetScopeProvider(scopeProvider);

        var logger = sut.CreateLogger("Test");
        logger.Should().BeOfType<TracesOnErrorLogger>();
        var tracesOnErrorLogger = (TracesOnErrorLogger)logger;
        tracesOnErrorLogger.ScopeProvider.Should().Be(scopeProvider);
    }

    [Fact]
    public void SetScopeProvider_AfterLoggerCreation_ScopeProviderIsSet()
    {
        var logSink = Substitute.For<ITracesOnErrorLogSink>();
        var storageProvider = Substitute.For<ITracesOnErrorStorageProvider>();
        var options = new TracesOnErrorOptions();
        var optionsMonitor = Substitute.For<IOptionsMonitor<TracesOnErrorOptions>>();
        optionsMonitor.CurrentValue.Returns(options);
        var sut = new TracesOnErrorLoggerProvider(logSink, storageProvider, optionsMonitor);

        var logger = sut.CreateLogger("Test");
        logger.Should().BeOfType<TracesOnErrorLogger>();
        var tracesOnErrorLogger = (TracesOnErrorLogger)logger;
        tracesOnErrorLogger.ScopeProvider.Should().BeNull();

        var scopeProvider = Substitute.For<IExternalScopeProvider>();

        sut.SetScopeProvider(scopeProvider);

        tracesOnErrorLogger.ScopeProvider.Should().Be(scopeProvider);
    }
}
