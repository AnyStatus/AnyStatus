using Microsoft.Extensions.Logging;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace AnyStatus.Core.Logging
{
    public class Logger : ILogger, IDisposable
    {
        private const int BufferSize = 100;

        private readonly ReplaySubject<LogEntry> _buffer = new(BufferSize);

        public IObservable<LogEntry> LogEntries => _buffer.AsObservable();

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _buffer.OnNext(new LogEntry
            {
                Time = DateTime.Now,
                LogLevel = logLevel,
                Exception = exception,
                Message = formatter(state, exception),
                ThreadId = Thread.CurrentThread.ManagedThreadId,
            });
        }

        public void Dispose()
        {
            _buffer.Dispose();
        }
    }
}
