using Microsoft.Extensions.Logging;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace AnyStatus.Core.Logging
{
    public class ActivityLogger : ILogger
    {
        private const int BufferSize = 100;

        private readonly ReplaySubject<ActivityMessage> _buffer = new(BufferSize);

        public IObservable<ActivityMessage> Messages => _buffer.AsObservable();

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _buffer.OnNext(new ActivityMessage
            {
                Time = DateTime.Now,
                LogLevel = logLevel,
                Exception = exception,
                Message = formatter(state, exception),
                ThreadId = Thread.CurrentThread.ManagedThreadId,
            });
        }
    }
}
