using AnyStatus.API.Attributes;
using AnyStatus.API.Endpoints;
using AnyStatus.API.Widgets;
using AnyStatus.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnyStatus.Core.Endpoints
{
    public class EndpointSource : IEndpointSource
    {
        private readonly IAppContext _context;

        public EndpointSource(IAppContext context) => _context = context;

        public IEnumerable<NameValueItem> GetItems(object source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var requiredEndpointType = GetRequiredEndpoint(source);

            return GetEndpointsAssignableFrom(requiredEndpointType);
        }

        private static Type GetRequiredEndpoint(object source)
            => source.GetType().GetInterfaces().Where(i => i.Name == typeof(IRequireEndpoint<>).Name).FirstOrDefault()?.GetGenericArguments().FirstOrDefault()
            ?? throw new InvalidOperationException("The required endpoint type was not found.");

        private List<NameValueItem> GetEndpointsAssignableFrom(Type requiredEndpoint)
            => _context.Endpoints?
                       .Where(endpoint => requiredEndpoint.IsAssignableFrom(endpoint.GetType()))
                       .Select(endpoint => new NameValueItem(endpoint.Name, endpoint.Id))
                       .ToList();
    }
}
