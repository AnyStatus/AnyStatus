using AnyStatus.API.Endpoints;
using System.Collections.ObjectModel;

namespace AnyStatus.Core.Domain
{
    public interface IAppContext
    {
        Session Session { get; set; }

        UserSettings UserSettings { get; set; }

        ObservableCollection<IEndpoint> Endpoints { get; set; }
    }
}
