using AnyStatus.API.Attributes;
using AnyStatus.API.Endpoints;
using AnyStatus.Core.Domain;
using System.Collections.Generic;
using System.Linq;

namespace AnyStatus.Core.Endpoints
{
    public class EndpointSource : IEndpointSource
    {
        private readonly IAppContext _context;

        public EndpointSource(IAppContext context) => _context = context;

        public IEnumerable<NameValueItem> GetItems(object source) => _context.Endpoints?.Select(endpoint => new NameValueItem(endpoint.Name, endpoint.Id)).ToList();
    }
}
