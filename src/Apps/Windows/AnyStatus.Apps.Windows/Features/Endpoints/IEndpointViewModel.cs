using AnyStatus.API.Endpoints;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid;

namespace AnyStatus.Apps.Windows.Features.Endpoints
{
    internal interface IEndpointViewModel
    {
        IEndpoint Endpoint { get; set; }

        IPropertyGridViewModel PropertyGridViewModel { get; set; }
    }
}