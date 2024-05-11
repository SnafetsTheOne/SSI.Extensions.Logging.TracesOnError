using FluentAssertions;
using System.Diagnostics;

namespace Snafets.Extensions.Logging.TracesOnError.Tests.Tests;

public class TracesOnErrorStorageProviderTests
{
    [Fact]
    public void DoNothing_NoLogs()
    {
        Activity.Current?.GetCustomProperty(TracesOnErrorStorageProvider.TracesOnErrorKey).Should().BeNull();
    }

    [Fact]
    public void GetLogs_NoSetup_Empty()
    {
        var sut = new TracesOnErrorStorageProvider();
        sut.GetLogs().Should().BeEmpty();

        Activity.Current?.GetCustomProperty(TracesOnErrorStorageProvider.TracesOnErrorKey).Should().BeNull();
    }

    [Fact]
    public void GetLogs_NoActivity_Transient()
    {
        var sut = new TracesOnErrorStorageProvider();
        var logs = sut.GetLogs();
        logs.Should().BeEmpty();
        logs.Add(new LogEntry());
        logs = sut.GetLogs();
        logs.Should().BeEmpty();
    }

    [Fact]
    public void GetLogs_Activity_Persistent()
    {
        using var activity = new Activity(nameof(GetLogs_Activity_Persistent));
        activity.Start();

        var sut = new TracesOnErrorStorageProvider();
        var logs = sut.GetLogs();
        logs.Add(new LogEntry());
        logs = sut.GetLogs();
        logs.Should().HaveCount(1);
    }
}

