using AnyStatus.API.Services;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Core.Logging;
using System;
using System.Collections.ObjectModel;

namespace AnyStatus.Apps.Windows.Features.Activity
{
    public sealed class ActivityViewModel : BaseViewModel, IDisposable
    {
        private readonly IDispatcher _dispatcher;
        private readonly IDisposable _subscription;

        public ActivityViewModel(Logger logger, IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;

            _subscription = logger.LogEntries.Subscribe(logEntry => _dispatcher.Invoke(() => LogEntries.Add(logEntry)));

            Commands.Add("Clear", new Command(_ => _dispatcher.Invoke(LogEntries.Clear), _ => LogEntries.Count > 0));
        }

        public void Dispose() => _subscription.Dispose();

        public ObservableCollection<LogEntry> LogEntries { get; } = new();
    }
}
