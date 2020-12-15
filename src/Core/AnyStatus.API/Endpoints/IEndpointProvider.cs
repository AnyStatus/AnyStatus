using System.Collections.Generic;

namespace AnyStatus.API.Endpoints
{
    public interface IEndpointProvider
    {
        IEnumerable<IEndpoint> GetEndpoints();

        IEndpoint GetEndpoint(string id);

        T GetEndpoint<T>(string id) where T : IEndpoint;
    }
}
