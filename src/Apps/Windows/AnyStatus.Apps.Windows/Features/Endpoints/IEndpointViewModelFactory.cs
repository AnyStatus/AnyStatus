using System;

namespace AnyStatus.Apps.Windows.Features.Endpoints
{
    internal interface IEndpointViewModelFactory
    {
        object Create(Type type);
    }
}
