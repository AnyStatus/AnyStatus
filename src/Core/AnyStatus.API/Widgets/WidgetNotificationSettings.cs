using AnyStatus.API.Common;

namespace AnyStatus.API.Widgets
{
    public class WidgetNotificationSettings : NotifyPropertyChanged
    {
        private bool _isEnabled;

        public bool IsEnabled
        {
            get => _isEnabled;
            set => Set(ref _isEnabled, value);
        }
    }
}
