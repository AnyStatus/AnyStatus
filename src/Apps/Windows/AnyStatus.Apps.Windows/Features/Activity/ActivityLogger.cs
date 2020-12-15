using AnyStatus.API.Services;
using AnyStatus.Core.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace AnyStatus.Apps.Windows.Features.Activity
{
    public class ActivityLogger : ILogger
    {
        private readonly IAppSettings _settings;
        private readonly IDispatcher _dispatcher;

        public ObservableCollection<ActivityMessage> Messages { get; } = new ObservableCollection<ActivityMessage>();

        public ActivityLogger(IAppSettings settings, IDispatcher dispatcher)
        {
            _settings = settings;
            _dispatcher = dispatcher;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (Messages.Count == _settings.MaxActivity)
            {
                _dispatcher.InvokeAsync(() => Messages.RemoveAt(0));
            }

            var message = new ActivityMessage
            {
                Time = DateTime.Now,
                LogLevel = logLevel,
                Exception = exception,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Message = formatter(state, exception)
            };

            _dispatcher.InvokeAsync(() => Messages.Add(message));
        }
    }
}
