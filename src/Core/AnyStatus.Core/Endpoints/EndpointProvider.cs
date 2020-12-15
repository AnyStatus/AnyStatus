using AnyStatus.API.Endpoints;
using AnyStatus.Core.Domain;
using System.Collections.Generic;
using System.Linq;

namespace AnyStatus.Core.Endpoints
{
    public class EndpointProvider : IEndpointProvider
    {
        private readonly IAppContext _context;

        public EndpointProvider(IAppContext context) => _context = context;

        public IEnumerable<IEndpoint> GetEndpoints() => _context.Endpoints;

        public IEndpoint GetEndpoint(string id) => _context.Endpoints?.FirstOrDefault(endpoint => endpoint.Id.Equals(id));

        public T GetEndpoint<T>(string id) where T : IEndpoint
        {
            return (T)_context.Endpoints?.FirstOrDefault(endpoint => endpoint.Id.Equals(id) && endpoint is T);
        }
    }
}
