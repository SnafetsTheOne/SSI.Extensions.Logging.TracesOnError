using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Snafets.Extensions.Logging.TracesOnError.Tests.Tests;

public class TracesOnErrorLoggerTests
{
    [Fact]
    public void Log_NoneLogLevel_NothingHappens()
    {
        var category = "category";
        var logSink = Substitute.For<ITracesOnErrorLogSink>();
        var storageProvider = Substitute.For<ITracesOnErrorStorageProvider>();
        var scopeProvider = Substitute.For<IExternalScopeProvider>();
        var options = new TracesOnErrorOptions();
        var sut = new TracesOnErrorLogger(category, logSink, storageProvider, scopeProvider, options);

        sut.Log(LogLevel.None, new EventId(1), "state", null, (s, e) => s.ToString());

        storageProvider.DidNotReceive().GetLogs();
    }

    [Fact]
    public void Log_BelowErrorThresholdLogLevelNoScopes_LogStored()
    {
        var category = "category";
        var logSink = Substitute.For<ITracesOnErrorLogSink>();
        var storageProvider = Substitute.For<ITracesOnErrorStorageProvider>();
        var list = new List<LogEntry>();
        storageProvider.When(x => x.AddLog(Arg.Any<LogEntry>())).Do(log => list.Add(log.Arg<LogEntry>()));
        storageProvider.GetLogs().Returns(list);
        var scopeProvider = Substitute.For<IExternalScopeProvider>();
        var options = new TracesOnErrorOptions();
        var sut = new TracesOnErrorLogger(category, logSink, storageProvider, scopeProvider, options);

        sut.Log(LogLevel.Trace, new EventId(1), "state", null, (s, e) => s.ToString());

        storageProvider.Received(1).AddLog(Arg.Any<LogEntry>());
        storageProvider.Received(0).GetLogs();
        list.Should().HaveCount(1);

        logSink.DidNotReceive().WriteLog(Arg.Any<IReadOnlyList<LogEntry>>());
    }

    [Fact]
    public void Log_AtErrorThresholdLogLevelNoScopes_LogStored()
    {
        var category = "category";
        var logSink = Substitute.For<ITracesOnErrorLogSink>();
        var storageProvider = Substitute.For<ITracesOnErrorStorageProvider>();
        var list = new List<LogEntry>();
        storageProvider.When(x => x.AddLog(Arg.Any<LogEntry>())).Do(log => list.Add(log.Arg<LogEntry>()));
        storageProvider.GetLogs().Returns(list);
        var scopeProvider = Substitute.For<IExternalScopeProvider>();
        var options = new TracesOnErrorOptions();
        var sut = new TracesOnErrorLogger(category, logSink, storageProvider, scopeProvider, options);

        sut.Log(LogLevel.Error, new EventId(1), "state", null, (s, e) => s.ToString());

        storageProvider.Received(1).GetLogs();
        list.Should().HaveCount(1);

        logSink.Received(1).WriteLog(Arg.Any<IReadOnlyList<LogEntry>>());
    }

    [Fact]
    public void Log_AboveErrorThresholdLogLevelNoScopes_LogStored()
    {
        var category = "category";
        var logSink = Substitute.For<ITracesOnErrorLogSink>();
        var storageProvider = Substitute.For<ITracesOnErrorStorageProvider>();
        var list = new List<LogEntry>();
        storageProvider.When(x => x.AddLog(Arg.Any<LogEntry>())).Do(log => list.Add(log.Arg<LogEntry>()));
        storageProvider.GetLogs().Returns(list);
        var scopeProvider = Substitute.For<IExternalScopeProvider>();
        var options = new TracesOnErrorOptions();
        var sut = new TracesOnErrorLogger(category, logSink, storageProvider, scopeProvider, options);

        sut.Log(LogLevel.Critical, new EventId(1), "state", null, (s, e) => s.ToString());

        storageProvider.Received(1).GetLogs();
        list.Should().HaveCount(1);

        logSink.Received(1).WriteLog(Arg.Any<IReadOnlyList<LogEntry>>());
    }

    [Fact]
    public void Log_AboveErrorThresholdLogLevelWithScopes_LogStored()
    {
        var category = "category";
        var logSink = Substitute.For<ITracesOnErrorLogSink>();
        var storageProvider = Substitute.For<ITracesOnErrorStorageProvider>();
        var list = new List<LogEntry>();
        storageProvider.When(x => x.AddLog(Arg.Any<LogEntry>())).Do(log => list.Add(log.Arg<LogEntry>()));
        storageProvider.GetLogs().Returns(list);
        var scopeProvider = Substitute.For<IExternalScopeProvider>();
        var options = new TracesOnErrorOptions()
        {
            IncludeScopes = true
        };
        var sut = new TracesOnErrorLogger(category, logSink, storageProvider, scopeProvider, options);

        sut.Log(LogLevel.Critical, new EventId(1), "state", null, (s, e) => s.ToString());

        storageProvider.Received(1).GetLogs();
        list.Should().HaveCount(1);

        scopeProvider.Received(1).ForEachScope(Arg.Any<Action<object?, List<object?>>>(), Arg.Any<List<object?>>());

        logSink.Received(1).WriteLog(Arg.Any<IReadOnlyList<LogEntry>>());
    }

    [Fact]
    public void BeginScope_ExcludeScope_NullInstance()
    {
        var category = "category";
        var logSink = Substitute.For<ITracesOnErrorLogSink>();
        var storageProvider = Substitute.For<ITracesOnErrorStorageProvider>();
        var list = new List<LogEntry>();
        storageProvider.When(x => x.AddLog(Arg.Any<LogEntry>())).Do(log => list.Add(log.Arg<LogEntry>()));
        storageProvider.GetLogs().Returns(list);
        var scopeProvider = Substitute.For<IExternalScopeProvider>();
        var options = new TracesOnErrorOptions()
        {
            IncludeScopes = false
        };
        var sut = new TracesOnErrorLogger(category, logSink, storageProvider, scopeProvider, options);

        var result = sut.BeginScope("state");
        result.Should().Be(NullScope.Instance);
    }

    [Fact]
    public void BeginScope_IncludeScopesNoScopeProvider_NullInstance()
    {
        var category = "category";
        var logSink = Substitute.For<ITracesOnErrorLogSink>();
        var storageProvider = Substitute.For<ITracesOnErrorStorageProvider>();
        var list = new List<LogEntry>();
        storageProvider.GetLogs().Returns(list);
        var options = new TracesOnErrorOptions()
        {
            IncludeScopes = true
        };
        var sut = new TracesOnErrorLogger(category, logSink, storageProvider, null, options);

        var result = sut.BeginScope("state");
        result.Should().Be(NullScope.Instance);
    }

    [Fact]
    public void BeginScope_IncludeScopesWithScopeProvider_NullInstance()
    {
        var category = "category";
        var logSink = Substitute.For<ITracesOnErrorLogSink>();
        var storageProvider = Substitute.For<ITracesOnErrorStorageProvider>();
        var list = new List<LogEntry>();
        storageProvider.When(x => x.AddLog(Arg.Any<LogEntry>())).Do(log => list.Add(log.Arg<LogEntry>()));
        storageProvider.GetLogs().Returns(list);
        var scopeProvider = Substitute.For<IExternalScopeProvider>();
        var options = new TracesOnErrorOptions()
        {
            IncludeScopes = true
        };
        var sut = new TracesOnErrorLogger(category, logSink, storageProvider, scopeProvider, options);

        var result = sut.BeginScope("state");
        result.Should().NotBe(NullScope.Instance);

        scopeProvider.Received(1).Push("state");
    }
}

