using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Core.Logging;
using System.Collections.ObjectModel;

namespace AnyStatus.Apps.Windows.Features.Activity
{
    public sealed class ActivityViewModel : BaseViewModel
    {
        private readonly ActivityLogger _logger;

        public ActivityViewModel(ActivityLogger logger)
        {
            _logger = logger;

            Commands.Add("Clear", new Command(_ => _logger.Messages.Clear(), _ => _logger.Messages?.Count > 0));
        }

        public ObservableCollection<ActivityMessage> Messages => _logger.Messages;
    }
}
