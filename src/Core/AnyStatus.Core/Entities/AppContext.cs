using AnyStatus.API.Common;
using AnyStatus.API.Endpoints;
using AnyStatus.Core.Sessions;
using AnyStatus.Core.Settings;
using System.Collections.ObjectModel;

namespace AnyStatus.Core.App
{
    public class AppContext : NotifyPropertyChanged, IAppContext
    {
        private Session _session;
        private UserSettings _userSettings;
        private ObservableCollection<IEndpoint> _endpoints;

        public Session Session
        {
            get => _session;
            set => Set(ref _session, value);
        }

        public UserSettings UserSettings
        {
            get => _userSettings;
            set => Set(ref _userSettings, value);
        }

        public ObservableCollection<IEndpoint> Endpoints
        {
            get => _endpoints;
            set => Set(ref _endpoints, value);
        }
    }
}
