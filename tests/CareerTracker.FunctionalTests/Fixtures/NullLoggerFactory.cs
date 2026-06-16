using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CareerTracker.FunctionalTests.Fixtures;

public class NullLoggerFactory : ILoggerFactory
{
    public void AddProvider(ILoggerProvider provider) { }
    public ILogger CreateLogger(string categoryName) => NullLogger.Instance;
    public void Dispose() { }
}

public class NullLogger : ILogger
{
    public static NullLogger Instance { get; } = new();
    public IDisposable BeginScope<TState>(TState? state) => NullDisposable.Instance;
    public bool IsEnabled(LogLevel logLevel) => false;
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) { }
}

public sealed class NullDisposable : IDisposable
{
    public static NullDisposable Instance { get; } = new();
    public void Dispose() { }
}
