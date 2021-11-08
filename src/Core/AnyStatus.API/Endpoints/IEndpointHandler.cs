namespace AnyStatus.API.Endpoints
{
    public interface IEndpointHandler<TEndpoint> where TEndpoint : IEndpoint
    {
        TEndpoint Endpoint { get; set; }
    }
}
