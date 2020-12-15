using AnyStatus.API.Endpoints;
using AnyStatus.Core.Domain;
using MediatR;
using System.Collections.ObjectModel;

namespace AnyStatus.Apps.Windows.Features.Endpoints
{
    internal class GetEndpoints
    {
        internal class Request : IRequest<Response>
        {
        }

        internal class Response
        {
            public Response(ObservableCollection<IEndpoint> endpoints)
            {
                Endpoints = endpoints;
            }

            public ObservableCollection<IEndpoint> Endpoints { get; }
        }

        internal class Handler : RequestHandler<Request, Response>
        {
            private readonly IAppContext _context;

            public Handler(IAppContext context)
            {
                _context = context;
            }

            protected override Response Handle(Request request)
            {
                return new Response(_context.Endpoints);
            }
        }
    }
}
