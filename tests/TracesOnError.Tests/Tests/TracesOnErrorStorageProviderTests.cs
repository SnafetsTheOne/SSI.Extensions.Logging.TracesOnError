using FluentAssertions;
using System.Diagnostics;

namespace Snafets.Extensions.Logging.TracesOnError.Tests.Tests;

public class TracesOnErrorStorageProviderTests
{
    [Fact]
    public void DoNothing_NoLogs()
    {
        Activity.Current?.GetCustomProperty(DefaultTracesOnErrorStorageProvider.TracesOnErrorKey).Should().BeNull();
    }

    [Fact]
    public void GetLogs_NoSetup_Empty()
    {
        var sut = new DefaultTracesOnErrorStorageProvider();
        sut.GetLogs().Should().BeEmpty();

        Activity.Current?.GetCustomProperty(DefaultTracesOnErrorStorageProvider.TracesOnErrorKey).Should().BeNull();
    }

    [Fact]
    public void GetLogs_NoActivity_Transient()
    {
        var sut = new DefaultTracesOnErrorStorageProvider();
        var logs = sut.GetLogs();
        logs.Should().BeEmpty();
        sut.AddLog(new LogEntry());
        logs = sut.GetLogs();
        logs.Should().BeEmpty();
    }

    [Fact]
    public void GetLogs_Activity_Persistent()
    {
        using var activity = new Activity(nameof(GetLogs_Activity_Persistent));
        activity.Start();

        var sut = new DefaultTracesOnErrorStorageProvider();
        sut.AddLog(new LogEntry());
        var logs = sut.GetLogs();
        logs.Should().HaveCount(1);
    }
}
