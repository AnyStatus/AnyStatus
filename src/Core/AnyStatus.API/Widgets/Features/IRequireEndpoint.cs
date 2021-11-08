using AnyStatus.API.Endpoints;

namespace AnyStatus.API.Widgets
{
    public interface IRequireEndpoint<TEndpoint> where TEndpoint : IEndpoint
    {
        string EndpointId { get; set; }
    }
}
