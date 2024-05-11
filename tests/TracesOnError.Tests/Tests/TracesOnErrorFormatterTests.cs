using FluentAssertions;

namespace SSI.Extensions.Logging.TracesOnError.Tests.Tests;

public class TracesOnErrorFormatterTests
{
    [Fact]
    public void Process_MessageLogEntry_CorrectString()
    {
        var logEntry = new LogEntry()
        {
            Category = "Category",
            EventId = 1,
            Message = "Message",
            Scopes = Array.Empty<string?>(),
        };
        var sut = new TracesOnErrorFormatter();

        var result = sut.Process(new List<LogEntry>() { logEntry });
        result.Should().Be("""
                           Traces:
                             Category[1]Message
                           
                           """);

    }

    [Fact]
    public void Process_ExceptionLogEntry_CorrectString()
    {
        var logEntry = new LogEntry()
        {
            Category = "Category",
            EventId = 1,
            Message = "Message",
            Scopes = Array.Empty<string?>(),
            Exception = new Exception("ErrorMessage"),
        };

        var sut = new TracesOnErrorFormatter();

        var result = sut.Process(new List<LogEntry>() { logEntry });

        result.Should().Be("""
                           Exception:
                             System.Exception: ErrorMessage

                           Traces:
                             Category[1]Message

                           """);
    }

    [Fact]
    public void Process_ThrownExceptionLogEntry_CorrectString()
    {
        var logEntry = new LogEntry()
        {
            Category = "Category",
            EventId = 1,
            Message = "Message",
            Scopes = Array.Empty<string?>(),
            Exception = CreateException("ErrorMessage"),
        };

        var sut = new TracesOnErrorFormatter();

        var result = sut.Process(new List<LogEntry>() { logEntry });

        result.Should().StartWith("""
                                  Exception:
                                    System.Exception: ErrorMessage
                                       at SSI.Extensions.Logging.TracesOnError.Tests.Tests.TracesOnErrorFormatterTests.CreateException(String message) in
                                  """);
        result.Should().EndWith("""
                                Traces:
                                  Category[1]Message
                                
                                """);
    }

    [Fact]
    public void Process_ScopedMessageLogEntry_CorrectString()
    {
        var logEntry = new LogEntry()
        {
            Category = "Category",
            EventId = 1,
            Message = "Message",
            Scopes = new List<string?> { "Scope" },
        };
        var sut = new TracesOnErrorFormatter();

        var result = sut.Process(new List<LogEntry>() { logEntry });
        result.Should().Be("""
                           Traces:
                             Category[1]Message<=Scope
                           
                           """);
    }

    [Fact]
    public void Process_ScopedExceptionLogEntry_CorrectString()
    {
        var logEntry = new LogEntry()
        {
            Category = "Category",
            EventId = 1,
            Message = "Message",
            Scopes = new List<string?> { "Scope" },
            Exception = new Exception("ErrorMessage"),
        };

        var sut = new TracesOnErrorFormatter();

        var result = sut.Process(new List<LogEntry>() { logEntry });

        result.Should().Be("""
                           Exception:
                             System.Exception: ErrorMessage

                           Traces:
                             Category[1]Message<=Scope

                           """);
    }

    [Fact]
    public void Process_ScopedThrownExceptionLogEntry_CorrectString()
    {
        var logEntry = new LogEntry()
        {
            Category = "Category",
            EventId = 1,
            Message = "Message",
            Scopes = new List<string?> { "Scope" },
            Exception = CreateException("ErrorMessage"),
        };

        var sut = new TracesOnErrorFormatter();

        var result = sut.Process(new List<LogEntry>() { logEntry });

        result.Should().StartWith("""
                                  Exception:
                                    System.Exception: ErrorMessage
                                       at SSI.Extensions.Logging.TracesOnError.Tests.Tests.TracesOnErrorFormatterTests.CreateException(String message) in
                                  """);
        result.Should().EndWith("""
                                Traces:
                                  Category[1]Message<=Scope
                                
                                """);
    }

    [Fact]
    public void ProcessMessageOnly_ScopedThrownExceptionLogEntry_CorrectString()
    {
        var logEntry = new LogEntry()
        {
            Category = "Category",
            EventId = 1,
            Message = "Message",
            Scopes = new List<string?> { "Scope" },
            Exception = CreateException("ErrorMessage"),
        };

        var sut = new TracesOnErrorFormatter();

        var result = sut.ProcessMessagesOnly(new List<LogEntry>() { logEntry });

        result.Should().Be("""
                           Traces:
                             Category[1]Message<=Scope

                           """);
    }

    private Exception CreateException(string message)
    {
        try
        {
            throw new Exception(message);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}
