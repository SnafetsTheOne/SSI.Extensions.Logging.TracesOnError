using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace SSI.Extensions.Logging.TracesOnError.Tests.Tests;

public class FormattedTracesOnErrorLogSinkTests
{
    [Fact]
    public void WriteLog_WhenCalled_CallsSinkWithFormattedLogs()
    {
        // Arrange
        var logs = new List<LogEntry>
        {
            new()
        };
        var formatter = Substitute.For<ITracesOnErrorFormatter>();
        var sink = Substitute.For<Action<string>>();
        var sut = new FormattedTracesOnErrorLogSink(formatter, sink);

        // Act
        sut.WriteLog(logs);

        // Assert
        sink.Received(1).Invoke(Arg.Any<string>());
    }
}

