using AnyStatus.API.Endpoints;
using AnyStatus.Core.Services;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace AnyStatus.Apps.Windows.Features.Endpoints
{
    internal class GetEndpointTypes
    {
        internal class Request : IRequest<Response>
        {
        }

        internal class Response
        {
            public Response(IEnumerable<EndpointTypeDescription> types) => Types = types;

            public IEnumerable<EndpointTypeDescription> Types { get; }
        }

        internal class Handler : RequestHandler<Request, Response>
        {
            protected override Response Handle(Request request)
            {
                var types = Scanner.GetTypesOf<IEndpoint>();

                var descriptions = new List<EndpointTypeDescription>();

                foreach (var type in types)
                {
                    var browsableAttribute = type.GetCustomAttribute<BrowsableAttribute>();

                    if (browsableAttribute?.Browsable == false)
                    {
                        continue;
                    }

                    var nameAttribute = type.GetCustomAttribute<DisplayNameAttribute>();

                    descriptions.Add(new EndpointTypeDescription
                    {
                        Type = type,
                        Name = string.IsNullOrWhiteSpace(nameAttribute?.DisplayName) ? type.Name : nameAttribute.DisplayName,
                    });
                }

                return new Response(descriptions);
            }
        }
    }
}
