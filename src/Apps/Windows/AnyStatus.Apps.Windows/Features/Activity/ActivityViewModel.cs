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

        public ActivityViewModel(ActivityLogger logger, IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;

            _subscription = logger.Messages.Subscribe(k => _dispatcher.Invoke(() => Messages.Add(k)));

            Commands.Add("Clear", new Command(_ => _dispatcher.Invoke(() => Messages.Clear()), _ => Messages.Count > 0));
        }

        public void Dispose() => _subscription.Dispose();

        public ObservableCollection<ActivityMessage> Messages { get; } = new();
    }
}
