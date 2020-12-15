using AnyStatus.API.Endpoints;

namespace AnyStatus.API.Widgets
{
    public interface IEndpointHandler<TEndpoint> where TEndpoint : IEndpoint
    {
        TEndpoint Endpoint { get; set; }
    }
}
